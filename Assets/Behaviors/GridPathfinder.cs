using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathfinder
{
    private enum OnList : byte
    {
        NONE = 0,
        OPEN,
        CLOSE,
    }

    public enum PathFinderState : byte
    {
        READY,
        WORKING,
        DONE,
    }

    public PathFinderState State { get; private set; } = PathFinderState.READY;
    public List<Vector2Int> Result { get; private set; } = null;

    private struct NodeInfo
    {
        public float givenCost;
        public float totalCost;
        public Vector2Int parentPos;
        public OnList onList;
        public int requestIteration;

        public void Reset()
        {
            totalCost = float.MaxValue;
            onList = OnList.NONE;
        }
    }

    private struct OpenListNode : System.IComparable
    {
        public float totalCost;
        public Vector2Int position;

        public static OpenListNode Create(float cost, Vector2Int pos)
        {
            var node = new OpenListNode();

            node.totalCost = cost;
            node.position = pos;

            return node;
        }

        public int CompareTo(object obj)
        {
            var other = (OpenListNode)obj;
            return totalCost.CompareTo(other.totalCost);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (OpenListNode)obj;
            return position.Equals(other.position);
        }
    }

    private TerrainInfo _terrainInfo = null;

    private Heap<OpenListNode> _openList = null;
    private NodeInfo[,] _allNodes = null;

    private Vector2Int _start;
    private Vector2Int _end;

    private int _requestCount = 0;

    private readonly Vector2Int[] _neighbors = new Vector2Int[]
    {
        new Vector2Int(-1, 1),  // repeat of last
        new Vector2Int(0, 1),   // first
        new Vector2Int(1, 1),
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),  // last
        new Vector2Int(0, 1),   // repeat of first
    };

    public GridPathfinder(in TerrainInfo terrainInfo)
    {
        _openList = new Heap<OpenListNode>();

        _terrainInfo = terrainInfo;
        _terrainInfo.OnMapChange += OnMapChangeListener;
    }

    ~GridPathfinder()
    {
        if (_terrainInfo)
        {
            _terrainInfo.OnMapChange -= OnMapChangeListener;
        }
    }

    private void OnMapChangeListener()
    {
        if (_allNodes == null ||
            _terrainInfo.Cols > _allNodes.GetLength(0) ||
            _terrainInfo.Rows > _allNodes.GetLength(1))
        {
            _allNodes = new NodeInfo[_terrainInfo.Cols, _terrainInfo.Rows];
        }
    }

    public void StartQuery(Vector2Int start, Vector2Int end)
    {
        Result = null;
        _start = start;
        _end = end;
        _requestCount++;
        _openList.Clear();

        var heuristicCost = GridHelper.GetOctileDistance(_start, _end);

        _allNodes[_start.x, _start.y].givenCost = 0;
        _allNodes[_start.x, _start.y].totalCost = heuristicCost;
        _allNodes[_start.x, _start.y].parentPos = new Vector2Int(-1, -1);
        _allNodes[_start.x, _start.y].onList = OnList.OPEN;
        _allNodes[_start.x, _start.y].requestIteration = _requestCount;

        var newOpenListNode = new OpenListNode();
        newOpenListNode.totalCost = heuristicCost;
        newOpenListNode.position = _start;

        _openList.Push(newOpenListNode);

        State = PathFinderState.WORKING;
    }

    public void ContinueQuery(TerrainVisualizer visualizer)
    {
        if (!_openList.IsEmpty)
        {
            var currentNode = _openList.Pop();
            ref var currentNodeInfo = ref _allNodes[currentNode.position.x, currentNode.position.y];

            if (currentNode.position == _end)
            {
                CreatePath();
                State = PathFinderState.DONE;
            }

            for (var  i = 1; i < _neighbors.Length - 1; ++i)
            {
                var neighbor = currentNode.position + _neighbors[i];

                if (!_terrainInfo.IsValidGrisPosition(neighbor) || _terrainInfo.IsWall(neighbor))
                {
                    continue;
                }
                else if ((i % 2) == 0)
                {
                    if (!_terrainInfo.IsValidGrisPosition(currentNode.position + _neighbors[i - 1]) ||
                        !_terrainInfo.IsValidGrisPosition(currentNode.position + _neighbors[i + 1]) ||
                        _terrainInfo.IsWall(currentNode.position + _neighbors[i - 1]) ||
                        _terrainInfo.IsWall(currentNode.position + _neighbors[i + 1]))
                    {
                        continue;
                    }
                }

                var distance = ((i % 2) == 1) ? 1.0f : 1.41421356237f;
                var newGivenCost = currentNodeInfo.givenCost + distance;
                var heuristicCost = GridHelper.GetOctileDistance(neighbor, _end);
                var newTotalCost = newGivenCost + heuristicCost * 1.05f;
                ref var neighborNode = ref _allNodes[neighbor.x, neighbor.y];

                if (neighborNode.requestIteration != _requestCount)
                {
                    neighborNode.Reset();
                    neighborNode.requestIteration = _requestCount;
                }

                if (neighborNode.totalCost > newTotalCost)
                {
                    neighborNode.parentPos = currentNode.position;
                    neighborNode.givenCost = newGivenCost;
                    neighborNode.totalCost = newTotalCost;

                    switch (neighborNode.onList)
                    {
                        case OnList.OPEN:
                            _openList.DecreaseKey(OpenListNode.Create(newTotalCost, neighbor));
                            break;
                        case OnList.NONE:
                            neighborNode.onList = OnList.OPEN;
                            _openList.Push(OpenListNode.Create(newTotalCost, neighbor));
                            visualizer.SetTileColor(neighbor, Color.green);
                            break;
                    }
                }
            }

            currentNodeInfo.onList = OnList.CLOSE;
            visualizer.SetTileColor(currentNode.position, Color.yellow);
        }
        else
        {
            Result = new List<Vector2Int>();
            State = PathFinderState.DONE;
        }
    }

    //public void CompleteQuery()
    //{
    //    while (State != PathFinderState.DONE)
    //    {
    //        ContinueQuery();
    //    }
    //}

    private void CreatePath()
    {
        var path = new List<Vector2Int>();

        var traverser = _end;
        while (traverser != _start)
        {
            path.Add(traverser);
            traverser = _allNodes[traverser.x, traverser.y].parentPos;
        }

        path.Reverse();

        Result = path;
        State = PathFinderState.DONE;
    }
}

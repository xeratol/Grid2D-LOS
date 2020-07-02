using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehavior : MonoBehaviour
{
    [SerializeField]
    private TerrainInfo _terrainInfo = null;

    [SerializeField]
    private TerrainVisualizer _terrainVisualizer = null;

    [SerializeField]
    private LineRenderer _lineRenderer = null;

    private GridPathfinder _pathfinder = null;

    void Start()
    {
        Debug.Assert(_terrainInfo, "Terrain Info not set", this);
        Debug.Assert(_terrainVisualizer, "Terrain Visualizer not set", this);
        Debug.Assert(_lineRenderer, "Line Renderer not set", this);

        _pathfinder = new GridPathfinder(_terrainInfo);

        _terrainInfo.OnMapChange += OnMapChangeListener;
    }

    private void OnMapChangeListener()
    {
        var agentGridPos = GridHelper.GetGridPosFromWorldPoint(transform.position);
        if (!_terrainInfo.IsValidGrisPosition(agentGridPos))
        {
            transform.position = Vector3.zero;
        }
    }

    void Update()
    {
        if (_pathfinder.State == GridPathfinder.PathFinderState.WORKING)
        {
            //_pathfinder.ContinueQuery();
            if (_pathfinder.State == GridPathfinder.PathFinderState.DONE)
            {
                var path = _pathfinder.Result;
                if (path != null)
                {
                    foreach (var pos in path)
                    {
                        _terrainVisualizer.SetTileColor(pos, Color.blue);
                    }
                }
                else
                {
                    Debug.Log("No valid path found");
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            var targetGridPos = GridHelper.GetGridPosFromScreenPoint(Input.mousePosition);
            if (_terrainInfo.IsValidGrisPosition(targetGridPos))
            {
                _terrainVisualizer.SetTileColor(targetGridPos, Color.cyan);
                var agentGridPos = GridHelper.GetGridPosFromWorldPoint(transform.position);
                _pathfinder.StartQuery(agentGridPos, targetGridPos);
                _lineRenderer.positionCount = 0;
                StartCoroutine(DebugPathFinder());
            }
        }
    }

    IEnumerator DebugPathFinder()
    {
        while (_pathfinder.State != GridPathfinder.PathFinderState.DONE)
        {
            yield return new WaitForSeconds(0.02f);
            _pathfinder.ContinueQuery(_terrainVisualizer);
        }

        var path = _pathfinder.Result;
        _lineRenderer.positionCount = path.Count;
        for (var i = 0; i < path.Count; ++i)
        {
            _lineRenderer.SetPosition(i, GridHelper.GetWorldPosFromGridPos(path[i]) + Vector3.up * 0.1f);
        }
    }
}

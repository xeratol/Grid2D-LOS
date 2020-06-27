using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathfinder : MonoBehaviour
{
    [SerializeField]
    private GridController gridController = null;

    void Start()
    {
        Debug.Assert(gridController, "Grid Controller not set", this);
    }

    List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        var path = new List<Vector2Int>();

        return path;
    }
}

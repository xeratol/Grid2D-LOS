using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainVisualizer : MonoBehaviour
{
    [SerializeField]
    private TerrainInfo _terrainInfo = null;

    [SerializeField]
    private Transform tilePrefab = null;

    private TileBehavior[,] _tiles = null;

    private void Start()
    {
        Debug.Assert(_terrainInfo, "Terrain Info not set", this);
        Debug.Assert(tilePrefab, "Tile Prefab not set");

        InitTiles();
        _terrainInfo.AddWallChangeListener(OnWallChangeListener);
    }

    private void OnDestroy()
    {
        if (_terrainInfo)
        {
            _terrainInfo.RemoveWallChangeListener(OnWallChangeListener);
        }
    }

    public void InitTiles()
    {
        _tiles = new TileBehavior[_terrainInfo.Cols, _terrainInfo.Rows];
        for (var c = 0; c < _terrainInfo.Cols; ++c)
        {
            for (var r = 0; r < _terrainInfo.Rows; ++r)
            {
                var tile = Instantiate(tilePrefab, transform);
                tile.position = new Vector3(c, 0, r);
                tile.name = string.Format("Tile {0} - {1}", c, r);

                _tiles[c, r] = tile.GetComponent<TileBehavior>();
            }
        }
    }

    public void ClearTiles()
    {
        foreach (var tile in _tiles)
        {
            Destroy(tile.gameObject);
        }
        _tiles = null;
    }

    public void UpdateTiles()
    {
        for (var c = 0; c < _terrainInfo.Cols; ++c)
        {
            for (var r = 0; r < _terrainInfo.Rows; ++r)
            {
                _tiles[c, r].state = _terrainInfo.IsWall(c, r) ? TileState.Wall : TileState.Open;
            }
        }
    }

    private void OnWallChangeListener()
    {
        // FIXME could be optimized (see capacity)

        ClearTiles();

        InitTiles();
        UpdateTiles();
    }
}

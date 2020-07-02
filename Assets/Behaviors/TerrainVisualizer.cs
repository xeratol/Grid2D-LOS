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
        Debug.Assert(tilePrefab, "Tile Prefab not set", this);

        InitTiles();
        _terrainInfo.OnMapChange += OnMapChangeListener;
    }

    private void OnDestroy()
    {
        if (_terrainInfo)
        {
            _terrainInfo.OnMapChange -= OnMapChangeListener;
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
        for (var col = 0; col < _terrainInfo.Cols; ++col)
        {
            for (var row = 0; row < _terrainInfo.Rows; ++row)
            {
                _tiles[col, row].state = _terrainInfo.IsWall(col, row) ? TileState.Wall : TileState.Open;
            }
        }
    }

    private void OnMapChangeListener()
    {
        // FIXME could be optimized (see capacity)

        ClearTiles();

        InitTiles();
        UpdateTiles();
    }

    //public TileBehavior GetTileAt(int col, int row)
    //{
    //    Debug.Assert(_terrainInfo.IsValidGrisPosition(col, row), "Out of bounds", this);

    //    return _tiles[col, row];
    //}

    public void SetTileColor(Vector2Int pos, Color color)
    {
        Debug.Assert(_terrainInfo.IsValidGrisPosition(pos), "Out of bounds", this);

        _tiles[pos.x, pos.y].SetColor(color);
    }
}

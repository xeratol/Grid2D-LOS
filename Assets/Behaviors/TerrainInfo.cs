using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInfo : MonoBehaviour
{
    [SerializeField]
    private int _rows = 10;
    public int Rows { get { return _rows; } }

    [SerializeField]
    private int _cols = 10;
    public int Cols { get { return _cols; } }

    private bool[,] _wallLayer = null;
    private bool _isDirty = false;

    public event Action OnMapChange;

    private void Start()
    {
        ResetWalls();
    }

    public void InitWalls(int cols, int rows)
    {
        _rows = rows;
        _cols = cols;
        _wallLayer = new bool[_cols, _rows];
        _isDirty = true;
    }

    public void ResetWalls()
    {
        _wallLayer = new bool[_cols, _rows];
        _isDirty = true;
    }

    public bool IsValidGrisPosition(int col, int row)
    {
        return row >= 0 && row < _rows && col >= 0 && col < _cols;
    }

    public bool IsValidGrisPosition(Vector2Int pos)
    {
        return IsValidGrisPosition(pos.x, pos.y);
    }

    public bool IsWall(int col, int row)
    {
        Debug.Assert(IsValidGrisPosition(col, row), "Out of bounds", this);

        return _wallLayer[col, row];
    }

    public bool IsWall(Vector2Int pos)
    {
        return IsWall(pos.x, pos.y);
    }

    public void SetWall(int col, int row)
    {
        Debug.Assert(IsValidGrisPosition(col, row), "Out of bounds", this);

        if (!_wallLayer[col, row])
        {
            _wallLayer[col, row] = true;
            _isDirty = true;
        }
    }

    public void SetWall(Vector2Int gridPos)
    {
        SetWall(gridPos.x, gridPos.y);
    }

    public void RemoveWall(int col, int row)
    {
        Debug.Assert(IsValidGrisPosition(col, row), "Out of bounds", this);

        if (_wallLayer[col, row])
        {
            _wallLayer[col, row] = false;
            _isDirty = true;
        }
    }

    public void RemoveWall(Vector2Int gridPos)
    {
        RemoveWall(gridPos.x, gridPos.y);
    }

    private void LateUpdate()
    {
        if (_isDirty)
        {
            OnMapChange?.Invoke();
            _isDirty = false;
        }
    }
}

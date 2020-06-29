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

    private Action _onWallChange;

    public void InitWalls(int rows, int cols)
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

    public bool IsWall(int row, int col)
    {
        Debug.Assert(row >= 0 && row < _rows, "Row is out of bounds", this);
        Debug.Assert(col >= 0 && col < _cols, "Col is out of bounds", this);

        return _wallLayer[col, row];
    }

    public void SetWall(int row, int col)
    {
        Debug.Assert(row >= 0 && row < _rows, "Row is out of bounds", this);
        Debug.Assert(col >= 0 && col < _cols, "Col is out of bounds", this);

        if (!_wallLayer[col, row])
        {
            _wallLayer[col, row] = true;
            _isDirty = true;
        }
    }

    public void RemoveWall(int row, int col)
    {
        Debug.Assert(row >= 0 && row < _rows, "Row is out of bounds", this);
        Debug.Assert(col >= 0 && col < _cols, "Col is out of bounds", this);

        if (_wallLayer[col, row])
        {
            _wallLayer[col, row] = false;
            _isDirty = true;
        }
    }

    public void AddWallChangeListener(Action listener)
    {
        _onWallChange += listener;
    }

    public void RemoveWallChangeListener(Action listener)
    {
        _onWallChange -= listener;
    }

    private void LateUpdate()
    {
        if (_isDirty)
        {
            _onWallChange?.Invoke();
            _isDirty = false;
        }
    }
}

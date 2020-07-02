using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainExporter : MonoBehaviour
{
    [SerializeField]
    private TerrainInfo _terrainInfo = null;

    [SerializeField]
    private InputField _exportText = null;

    void Start()
    {
        Debug.Assert(_terrainInfo, "Terrain Info not set", this);
        Debug.Assert(_exportText, "Export Text not set", this);
    }

    public void ExportMap()
    {
        var rows = _terrainInfo.Rows;
        var cols = _terrainInfo.Cols;

        var text = "";
        text += "rows = " + rows + ";" + System.Environment.NewLine;
        text += "cols = " + cols + ";" + System.Environment.NewLine;

        for (var col = 0; col < cols; ++col)
        {
            for (var row = 0; row < rows; ++row)
            {
                if (_terrainInfo.IsWall(col, row))
                {
                    text += "_terrainInfo.SetWall(" + col + ", " + row + ");" + System.Environment.NewLine;
                }
            }
        }

        _exportText.text = text;
    }
}

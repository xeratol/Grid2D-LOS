using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    [SerializeField]
    private TerrainInfo _terrainInfo = null;

    void Start()
    {
        Debug.Assert(_terrainInfo, "Terrain Info not set", this);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var gridPos = GridHelper.GetGridPosFromScreenPoint(Input.mousePosition);
            if (_terrainInfo.IsValidGrisPosition(gridPos))
            {
                if (_terrainInfo.IsWall(gridPos))
                {
                    _terrainInfo.RemoveWall(gridPos);
                }
                else
                {
                    _terrainInfo.SetWall(gridPos);
                }
            }
        }
    }
}

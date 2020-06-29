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
            var gridPos = GetGridPosFromScreenPoint(Input.mousePosition);
            if (_terrainInfo.IsValidGrisPosition(gridPos.x, gridPos.y))
            {
                if (_terrainInfo.IsWall(gridPos.x, gridPos.y))
                {
                    _terrainInfo.RemoveWall(gridPos.x, gridPos.y);
                }
                else
                {
                    _terrainInfo.SetWall(gridPos.x, gridPos.y);
                }
            }
        }
    }

    public Vector2Int GetGridPosFromScreenPoint(Vector3 screenPoint)
    {
        var ray = Camera.main.ScreenPointToRay(screenPoint);
        var plane = new Plane(Vector3.up, 0);
        var t = 0.0f;

        Debug.Assert(plane.Raycast(ray, out t), "Impossible Error", this);

        var pt = ray.GetPoint(t);
        var c = Mathf.RoundToInt(pt.x);
        var r = Mathf.RoundToInt(pt.z);

        return new Vector2Int(c, r);
    }
}

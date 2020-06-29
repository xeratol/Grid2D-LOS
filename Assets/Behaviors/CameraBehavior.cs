using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField]
    private TerrainInfo _terrainInfo = null;

    void Start()
    {
        Debug.Assert(_terrainInfo, "Terrain Info not set", this);

        SetupCamera();

        _terrainInfo.AddWallChangeListener(OnWallChangeListener);
    }

    private void OnWallChangeListener()
    {
        SetupCamera();
    }

    private void SetupCamera()
    {
        var rows = _terrainInfo.Rows;
        var cols = _terrainInfo.Cols;

        Camera.main.orthographic = true;
        Camera.main.orthographicSize = (Mathf.Max(rows, cols) + 2) * 0.5f;
        Camera.main.transform.position =
            new Vector3((cols - 1) * 0.5f, Mathf.Max(rows, cols), (rows - 1) * 0.5f);
    }
}

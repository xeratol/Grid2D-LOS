using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField]
    private TerrainInfo _terrainInfo = null;

    void Start()
    {
        Debug.Assert(_terrainInfo, "Terrain Info not set", this);

        SetupCamera();

        _terrainInfo.OnMapChange += OnMapChangeListener;
    }

    void OnDestroy()
    {
        if (_terrainInfo)
        {
            _terrainInfo.OnMapChange -= OnMapChangeListener;
        }
    }

    private void OnMapChangeListener()
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

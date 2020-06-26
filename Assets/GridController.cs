using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private int rows = 10;

    [SerializeField]
    private int cols = 10;

    [SerializeField]
    private Transform tilePrefab = null;

    public TileBehavior[][] tiles { get; private set; }
    private Vector2Int [] poi = new Vector2Int[2];

    [SerializeField]
    private InputField exportText = null;

    [SerializeField]
    private LineRenderer lineRenderer = null;

    private void Start()
    {
        Debug.Assert(tilePrefab, "Tile Prefab not set");
        Debug.Assert(rows > 0, "Invalid number of rows");
        Debug.Assert(cols > 0, "Invalid number of rows");

        SetupGrid();
        SetupCamera();
    }

    private void SetupCamera()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = (Mathf.Max(rows, cols) + 1) * 0.5f;
        Camera.main.transform.position =
            new Vector3((cols - 1) * 0.5f, Mathf.Max(rows, cols), (rows - 1) * 0.5f);
    }

    private void SetupGrid()
    {
        tiles = new TileBehavior[cols][];

        for (var c = 0; c < cols; ++c)
        {
            tiles[c] = new TileBehavior[rows];
            for (var r = 0; r < rows; ++r)
            {
                var tile = Instantiate(tilePrefab, transform);
                tile.position = new Vector3(c, 0, r);
                tile.name = string.Format("Tile {0} - {1}", c, r);

                tiles[c][r] = tile.GetComponent<TileBehavior>();
            }
        }
    }

    private void ClearGrid()
    {
        foreach (var tileRow in tiles)
        {
            foreach (var tile in tileRow)
            {
                Destroy(tile.gameObject);
            }
        }

        tiles = null;
        poi = new Vector2Int[2];

        if (lineRenderer)
        {
            lineRenderer.positionCount = 0;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpdateTileWall();
            ResetVisibility();
            UpdateVisibility();
        }
    }

    private void ResetVisibility()
    {
        foreach (var tileRow in tiles)
        {
            foreach (var tile in tileRow)
            {
                tile.visibility = Visibility.Unknown;
            }
        }
    }

    private void UpdateVisibility()
    {
        // http://playtechs.blogspot.com/2007/03/raytracing-on-grid.html

        var blocked = false;

        var diff = poi[1] - poi[0];
        var inc = new Vector2Int((diff.x > 0) ? 1 : -1, (diff.y > 0) ? 1 : -1);
        diff.x = Mathf.Abs(diff.x);
        diff.y = Mathf.Abs(diff.y);
        var point = poi[0];
        var error = diff.x - diff.y;
        diff *= 2;

        while (point != poi[1])
        {
            UpdateTileForVisiblity(point, ref blocked);
            if (error > 0)
            {
                point.x += inc.x;
                error -= diff.y;
            }
            else if (error < 0)
            {
                point.y += inc.y;
                error += diff.x;
            }
            else // error == 0
            {
                if (diff.x < diff.y)
                {
                    // horizontal then vertical
                    UpdateTileForVisiblity(new Vector2Int(point.x + inc.x, point.y), ref blocked);
                    UpdateTileForVisiblity(new Vector2Int(point.x, point.y + inc.y), ref blocked);
                }
                else
                {
                    // vertical then horizontal
                    UpdateTileForVisiblity(new Vector2Int(point.x, point.y + inc.y), ref blocked);
                    UpdateTileForVisiblity(new Vector2Int(point.x + inc.x, point.y), ref blocked);
                }
                point += inc;
                error += -diff.y + diff.x;
            }
        }
        UpdateTileForVisiblity(point, ref blocked);
    }

    private void UpdateTileForVisiblity(Vector2Int point, ref bool blocked)
    {
        //Debug.LogFormat("{0} - {1}", point, blocked);
        var tile = tiles[point.x][point.y];
        if (tile.state == TileState.Open)
        {
            if (tile.visibility == Visibility.Unknown)
            {
                if (blocked)
                {
                    tile.visibility = Visibility.Hidden;
                }
                else
                {
                    tile.visibility = Visibility.Visible;
                }
            }
            else if (tile.visibility == Visibility.Hidden)
            {
                blocked = true;
            }
        }
        else if (tile.state == TileState.Wall)
        {
            blocked = true;
        }
    }

    private void UpdateTileWall()
    {
        var tile = GetTileFromScreenPoint(Input.mousePosition);
        if (tile)
        {
            if (tile.state != TileState.POI)
            {
                tile.state =
                    (tile.state == TileState.Open) ? TileState.Wall : TileState.Open;
            }
        }
    }

    private TileBehavior GetTileFromScreenPoint(Vector3 screenPoint)
    {
        var ray = Camera.main.ScreenPointToRay(screenPoint);
        var plane = new Plane(Vector3.up, 0);
        var t = 0.0f;
        if (plane.Raycast(ray, out t))
        {
            var pt = ray.GetPoint(t);
            var c = Mathf.RoundToInt(pt.x);
            var r = Mathf.RoundToInt(pt.z);

            if (c >= 0 && c< cols && r >= 0 && r < rows)
            {
                return tiles[c][r];
            }
        }

        return null;
    }

    #region Preset Maps
    public void LoadMap01()
    {
        ClearGrid();

        rows = 20;
        cols = 20;
        SetupGrid();
        SetupCamera();

        StartCoroutine(LoadMap01More());
    }

    private IEnumerator LoadMap01More()
    {
        yield return 0; // Renderer component is not ready on the same frame

        tiles[4][6].state = TileState.Wall;
        tiles[4][7].state = TileState.Wall;
        tiles[4][8].state = TileState.Wall;
        tiles[4][9].state = TileState.Wall;
        tiles[4][10].state = TileState.Wall;
        tiles[4][11].state = TileState.Wall;
        tiles[4][12].state = TileState.Wall;
        tiles[4][13].state = TileState.Wall;
        tiles[5][6].state = TileState.Wall;
        tiles[6][6].state = TileState.Wall;
        tiles[7][6].state = TileState.Wall;
        tiles[8][6].state = TileState.Wall;
        tiles[8][13].state = TileState.Wall;
        tiles[9][6].state = TileState.Wall;
        tiles[9][13].state = TileState.Wall;
        tiles[10][6].state = TileState.Wall;
        tiles[10][13].state = TileState.Wall;
        tiles[11][6].state = TileState.Wall;
        tiles[11][13].state = TileState.Wall;
        tiles[12][13].state = TileState.Wall;
        tiles[13][13].state = TileState.Wall;
        tiles[14][13].state = TileState.Wall;
        tiles[15][6].state = TileState.Wall;
        tiles[15][7].state = TileState.Wall;
        tiles[15][8].state = TileState.Wall;
        tiles[15][9].state = TileState.Wall;
        tiles[15][10].state = TileState.Wall;
        tiles[15][11].state = TileState.Wall;
        tiles[15][12].state = TileState.Wall;
        tiles[15][13].state = TileState.Wall;
    }

    public void LoadMap02()
    {
        ClearGrid();

        rows = 40;
        cols = 40;
        SetupGrid();
        SetupCamera();

        StartCoroutine(LoadMap02More());
    }

    private IEnumerator LoadMap02More()
    {
        yield return 0; // Renderer component is not ready on the same frame

        tiles[0][10].state = TileState.Wall;
        tiles[0][17].state = TileState.Wall;
        tiles[0][27].state = TileState.Wall;
        tiles[1][10].state = TileState.Wall;
        tiles[1][17].state = TileState.Wall;
        tiles[1][27].state = TileState.Wall;
        tiles[2][10].state = TileState.Wall;
        tiles[2][17].state = TileState.Wall;
        tiles[2][27].state = TileState.Wall;
        tiles[3][10].state = TileState.Wall;
        tiles[3][17].state = TileState.Wall;
        tiles[3][27].state = TileState.Wall;
        tiles[4][10].state = TileState.Wall;
        tiles[4][27].state = TileState.Wall;
        tiles[5][10].state = TileState.Wall;
        tiles[5][17].state = TileState.Wall;
        tiles[5][27].state = TileState.Wall;
        tiles[6][10].state = TileState.Wall;
        tiles[6][17].state = TileState.Wall;
        tiles[6][27].state = TileState.Wall;
        tiles[7][10].state = TileState.Wall;
        tiles[7][17].state = TileState.Wall;
        tiles[7][27].state = TileState.Wall;
        tiles[8][10].state = TileState.Wall;
        tiles[8][17].state = TileState.Wall;
        tiles[8][27].state = TileState.Wall;
        tiles[9][10].state = TileState.Wall;
        tiles[9][17].state = TileState.Wall;
        tiles[9][27].state = TileState.Wall;
        tiles[10][17].state = TileState.Wall;
        tiles[10][27].state = TileState.Wall;
        tiles[11][10].state = TileState.Wall;
        tiles[11][17].state = TileState.Wall;
        tiles[12][10].state = TileState.Wall;
        tiles[12][17].state = TileState.Wall;
        tiles[12][27].state = TileState.Wall;
        tiles[13][10].state = TileState.Wall;
        tiles[13][17].state = TileState.Wall;
        tiles[13][27].state = TileState.Wall;
        tiles[14][10].state = TileState.Wall;
        tiles[14][17].state = TileState.Wall;
        tiles[14][27].state = TileState.Wall;
        tiles[15][10].state = TileState.Wall;
        tiles[15][11].state = TileState.Wall;
        tiles[15][12].state = TileState.Wall;
        tiles[15][13].state = TileState.Wall;
        tiles[15][14].state = TileState.Wall;
        tiles[15][15].state = TileState.Wall;
        tiles[15][16].state = TileState.Wall;
        tiles[15][17].state = TileState.Wall;
        tiles[15][18].state = TileState.Wall;
        tiles[15][19].state = TileState.Wall;
        tiles[15][20].state = TileState.Wall;
        tiles[15][21].state = TileState.Wall;
        tiles[15][22].state = TileState.Wall;
        tiles[15][23].state = TileState.Wall;
        tiles[15][24].state = TileState.Wall;
        tiles[15][25].state = TileState.Wall;
        tiles[15][26].state = TileState.Wall;
        tiles[15][27].state = TileState.Wall;
        tiles[15][28].state = TileState.Wall;
        tiles[15][29].state = TileState.Wall;
        tiles[15][30].state = TileState.Wall;
        tiles[15][31].state = TileState.Wall;
        tiles[15][33].state = TileState.Wall;
        tiles[15][34].state = TileState.Wall;
        tiles[16][14].state = TileState.Wall;
        tiles[16][21].state = TileState.Wall;
        tiles[16][34].state = TileState.Wall;
        tiles[17][14].state = TileState.Wall;
        tiles[17][21].state = TileState.Wall;
        tiles[17][34].state = TileState.Wall;
        tiles[18][21].state = TileState.Wall;
        tiles[18][34].state = TileState.Wall;
        tiles[19][14].state = TileState.Wall;
        tiles[19][21].state = TileState.Wall;
        tiles[19][34].state = TileState.Wall;
        tiles[20][14].state = TileState.Wall;
        tiles[20][21].state = TileState.Wall;
        tiles[20][34].state = TileState.Wall;
        tiles[21][14].state = TileState.Wall;
        tiles[21][21].state = TileState.Wall;
        tiles[21][34].state = TileState.Wall;
        tiles[22][14].state = TileState.Wall;
        tiles[22][21].state = TileState.Wall;
        tiles[22][34].state = TileState.Wall;
        tiles[23][14].state = TileState.Wall;
        tiles[23][21].state = TileState.Wall;
        tiles[23][34].state = TileState.Wall;
        tiles[24][14].state = TileState.Wall;
        tiles[24][21].state = TileState.Wall;
        tiles[24][34].state = TileState.Wall;
        tiles[25][14].state = TileState.Wall;
        tiles[25][21].state = TileState.Wall;
        tiles[25][22].state = TileState.Wall;
        tiles[25][23].state = TileState.Wall;
        tiles[25][24].state = TileState.Wall;
        tiles[25][25].state = TileState.Wall;
        tiles[25][26].state = TileState.Wall;
        tiles[25][27].state = TileState.Wall;
        tiles[25][28].state = TileState.Wall;
        tiles[25][30].state = TileState.Wall;
        tiles[25][31].state = TileState.Wall;
        tiles[25][32].state = TileState.Wall;
        tiles[25][33].state = TileState.Wall;
        tiles[25][34].state = TileState.Wall;
        tiles[26][14].state = TileState.Wall;
        tiles[26][21].state = TileState.Wall;
        tiles[27][14].state = TileState.Wall;
        tiles[27][21].state = TileState.Wall;
        tiles[28][14].state = TileState.Wall;
        tiles[28][21].state = TileState.Wall;
        tiles[29][14].state = TileState.Wall;
        tiles[29][21].state = TileState.Wall;
        tiles[30][14].state = TileState.Wall;
        tiles[30][21].state = TileState.Wall;
        tiles[31][14].state = TileState.Wall;
        tiles[32][14].state = TileState.Wall;
        tiles[32][21].state = TileState.Wall;
        tiles[33][14].state = TileState.Wall;
        tiles[33][21].state = TileState.Wall;
        tiles[34][14].state = TileState.Wall;
        tiles[34][21].state = TileState.Wall;
        tiles[35][14].state = TileState.Wall;
        tiles[35][21].state = TileState.Wall;
        tiles[36][14].state = TileState.Wall;
        tiles[36][21].state = TileState.Wall;
        tiles[37][14].state = TileState.Wall;
        tiles[37][21].state = TileState.Wall;
        tiles[38][14].state = TileState.Wall;
        tiles[38][21].state = TileState.Wall;
        tiles[39][14].state = TileState.Wall;
        tiles[39][21].state = TileState.Wall;
    }

    public void LoadMap03()
    {
        ClearGrid();

        rows = 40;
        cols = 40;
        SetupGrid();
        SetupCamera();

        StartCoroutine(LoadMap03More());
    }

    private IEnumerator LoadMap03More()
    {
        yield return 0; // Renderer component is not ready on the same frame

        tiles[0][27].state = TileState.Wall;
        tiles[0][32].state = TileState.Wall;
        tiles[0][39].state = TileState.Wall;
        tiles[1][4].state = TileState.Wall;
        tiles[1][12].state = TileState.Wall;
        tiles[1][18].state = TileState.Wall;
        tiles[1][24].state = TileState.Wall;
        tiles[1][29].state = TileState.Wall;
        tiles[1][35].state = TileState.Wall;
        tiles[1][37].state = TileState.Wall;
        tiles[2][9].state = TileState.Wall;
        tiles[2][16].state = TileState.Wall;
        tiles[2][27].state = TileState.Wall;
        tiles[2][30].state = TileState.Wall;
        tiles[2][34].state = TileState.Wall;
        tiles[3][21].state = TileState.Wall;
        tiles[3][38].state = TileState.Wall;
        tiles[4][26].state = TileState.Wall;
        tiles[4][28].state = TileState.Wall;
        tiles[4][31].state = TileState.Wall;
        tiles[4][36].state = TileState.Wall;
        tiles[5][4].state = TileState.Wall;
        tiles[5][8].state = TileState.Wall;
        tiles[5][12].state = TileState.Wall;
        tiles[5][18].state = TileState.Wall;
        tiles[5][23].state = TileState.Wall;
        tiles[5][33].state = TileState.Wall;
        tiles[5][39].state = TileState.Wall;
        tiles[6][1].state = TileState.Wall;
        tiles[6][30].state = TileState.Wall;
        tiles[6][36].state = TileState.Wall;
        tiles[7][15].state = TileState.Wall;
        tiles[7][27].state = TileState.Wall;
        tiles[7][35].state = TileState.Wall;
        tiles[7][38].state = TileState.Wall;
        tiles[8][21].state = TileState.Wall;
        tiles[8][30].state = TileState.Wall;
        tiles[8][33].state = TileState.Wall;
        tiles[8][39].state = TileState.Wall;
        tiles[9][7].state = TileState.Wall;
        tiles[9][11].state = TileState.Wall;
        tiles[9][25].state = TileState.Wall;
        tiles[9][36].state = TileState.Wall;
        tiles[10][15].state = TileState.Wall;
        tiles[10][19].state = TileState.Wall;
        tiles[10][38].state = TileState.Wall;
        tiles[11][25].state = TileState.Wall;
        tiles[11][27].state = TileState.Wall;
        tiles[12][23].state = TileState.Wall;
        tiles[12][32].state = TileState.Wall;
        tiles[12][35].state = TileState.Wall;
        tiles[12][37].state = TileState.Wall;
        tiles[13][30].state = TileState.Wall;
        tiles[13][38].state = TileState.Wall;
        tiles[14][6].state = TileState.Wall;
        tiles[14][15].state = TileState.Wall;
        tiles[14][20].state = TileState.Wall;
        tiles[15][24].state = TileState.Wall;
        tiles[15][33].state = TileState.Wall;
        tiles[16][29].state = TileState.Wall;
        tiles[16][36].state = TileState.Wall;
        tiles[16][38].state = TileState.Wall;
        tiles[17][2].state = TileState.Wall;
        tiles[18][19].state = TileState.Wall;
        tiles[18][24].state = TileState.Wall;
        tiles[18][39].state = TileState.Wall;
        tiles[19][10].state = TileState.Wall;
        tiles[19][30].state = TileState.Wall;
        tiles[19][32].state = TileState.Wall;
        tiles[19][35].state = TileState.Wall;
        tiles[20][37].state = TileState.Wall;
        tiles[21][21].state = TileState.Wall;
        tiles[21][26].state = TileState.Wall;
        tiles[21][33].state = TileState.Wall;
        tiles[22][1].state = TileState.Wall;
        tiles[23][7].state = TileState.Wall;
        tiles[23][28].state = TileState.Wall;
        tiles[23][36].state = TileState.Wall;
        tiles[23][38].state = TileState.Wall;
        tiles[25][15].state = TileState.Wall;
        tiles[26][23].state = TileState.Wall;
        tiles[26][28].state = TileState.Wall;
        tiles[26][34].state = TileState.Wall;
        tiles[27][3].state = TileState.Wall;
        tiles[27][8].state = TileState.Wall;
        tiles[27][36].state = TileState.Wall;
        tiles[28][38].state = TileState.Wall;
        tiles[29][33].state = TileState.Wall;
        tiles[30][20].state = TileState.Wall;
        tiles[31][7].state = TileState.Wall;
        tiles[31][37].state = TileState.Wall;
        tiles[32][28].state = TileState.Wall;
        tiles[33][14].state = TileState.Wall;
        tiles[33][34].state = TileState.Wall;
        tiles[36][4].state = TileState.Wall;
        tiles[36][23].state = TileState.Wall;
        tiles[37][28].state = TileState.Wall;
        tiles[37][34].state = TileState.Wall;
        tiles[38][15].state = TileState.Wall;
    }
    #endregion

    #region Map Operations
    public void ClearMap()
    {
        foreach (var tileRow in tiles)
        {
            foreach (var tile in tileRow)
            {
                tile.state = TileState.Open;
                tile.visibility = Visibility.Unknown;
            }
        }
        poi = new Vector2Int[2];

        if (lineRenderer)
        {
            lineRenderer.positionCount = 0;
        }
    }

    public void ExportMap()
    {
        var text = "";
        text += "rows = " + rows + ";" + System.Environment.NewLine;
        text += "cols = " + cols + ";" + System.Environment.NewLine;

        for (var c = 0; c < rows; ++c)
        {
            for (var r = 0; r < rows; ++r)
            {
                if (tiles[c][r].state == TileState.Wall)
                {
                    text += "tiles[" + c + "][" + r + "].state = TileState." + tiles[c][r].state.ToString() + ";" + System.Environment.NewLine;
                }
            }
        }

        exportText.gameObject.SetActive(true);
        exportText.text = text;
    }
    #endregion

    public void SetPOI(Vector3 worldPos, int index)
    {
        Debug.Assert(index < poi.Length, "Invalid index", this);

        poi[index] = new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.z));
        poi[index].x = Mathf.Clamp(poi[index].x, 0, cols);
        poi[index].y = Mathf.Clamp(poi[index].y, 0, rows);

        if (lineRenderer)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, new Vector3(poi[0].x, 0.1f, poi[0].y));
            lineRenderer.SetPosition(1, new Vector3(poi[1].x, 0.1f, poi[1].y));
        }

        ResetVisibility();
        UpdateVisibility();
    }
}

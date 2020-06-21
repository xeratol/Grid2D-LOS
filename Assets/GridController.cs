using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private int rows = 10;

    [SerializeField]
    private int cols = 10;

    [SerializeField]
    private Transform tilePrefab = null;

    private TileBehavior[][] tiles;
    private Vector2Int poi = new Vector2Int(-1, -1);

    private void Start()
    {
        Debug.Assert(tilePrefab, "Tile Prefab not set");
        Debug.Assert(rows > 0, "Invalid number of rows");
        Debug.Assert(cols > 0, "Invalid number of rows");

        SetupGrid();

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpdateTileWall();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SelectPOI();
        }
    }

    private void SelectPOI()
    {
        var tile = GetTileFromScreenPoint(Input.mousePosition);
        if (tile)
        {
            if (tile.state == TileState.Open)
            {
                if (poi.x != -1 && poi.y != -1)
                {
                    tiles[poi.x][poi.y].state = TileState.Open;
                }

                tile.state = TileState.POI;
                poi = new Vector2Int((int)tile.transform.position.x, (int)tile.transform.position.z);
            }

            // Update Visibility
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
}

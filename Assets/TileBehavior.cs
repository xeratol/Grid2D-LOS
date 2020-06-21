using UnityEngine;

public enum TileState
{
    Open,
    Wall,
    POI
}

public class TileBehavior : MonoBehaviour
{
    private TileState _state;
    public TileState state
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            UpdateColorBasedOnState();
        }
    }

    private void UpdateColorBasedOnState()
    {
        switch (_state)
        {
            case TileState.Open:
                _material.color = Color.white;
                break;
            case TileState.Wall:
                _material.color = Color.black;
                break;
            case TileState.POI:
                _material.color = Color.blue;
                break;
        }
    }

    private Material _material;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

}

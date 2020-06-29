using UnityEngine;

public enum TileState
{
    Open,
    Wall,
    POI,
}

public enum Visibility
{
    Unknown,
    Visible,
    Hidden,
}

public class TileBehavior : MonoBehaviour
{
    private TileState _state = TileState.Open;
    public TileState state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != value)
            {
                _state = value;
                UpdateColorBasedOnState();
            }
        }
    }

    private Visibility _visibility;
    public Visibility visibility
    {
        get
        {
            return _visibility;
        }
        set
        {
            if (_visibility != value)
            {
                _visibility = value;
                UpdateColorBasedOnState();
            }
        }
    }

    private void UpdateColorBasedOnState()
    {
        switch (_state)
        {
            case TileState.Open:
                if (_visibility == Visibility.Visible)
                {
                    _material.color = Color.blue;
                }
                else
                {
                    _material.color = Color.white;
                }
                break;
            case TileState.Wall:
                _material.color = Color.black;
                break;
            case TileState.POI:
                _material.color = Color.blue;
                break;
        }
    }

    private Material _material = null;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

}

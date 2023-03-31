using UnityEngine;

public class Hex {
    private readonly Vector2Int _index;
    public Vector2Int index => _index;

    private readonly float _height;
    public float height => _height;

    private HexType _type;
    public HexType type => _type;
    
    private HexStateType _state;
    public HexStateType state => _state;

    private CharController _occupant;
    public CharController occupant => _occupant;
    
    public Hex(Vector2Int index, float height, HexType type) {
        _index = index;
        _height = height;
        _type = type;
        _occupant = null;
    }

    public void Occupy(CharController occupant) {
        _occupant = occupant;
        _state = HexStateType.Occupied;
    }

    public void Free() {
        _occupant = null;
        _state = HexStateType.Empty;
    }
}
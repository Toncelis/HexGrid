using UnityEngine;

public class Char {
    private Vector2Int _index;
    public Vector2Int index => _index;

    public Char(HexController hex) {
        _index = hex.Model.index;
    }

    public void SetHex(HexController hex) {
        _index = hex.Model.index;
    }
}
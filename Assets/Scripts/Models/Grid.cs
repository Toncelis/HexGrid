using System.Collections.Generic;
using UnityEngine;

public class Grid {
    private readonly Dictionary<Vector2Int,HexController> _hexes;

    private readonly GridSettings _settings;
    
    public GridSettings settings => _settings;

    public Grid(GridSettings settings) {
        _settings = settings;

        _hexes = new();
    }
    
    
    public void AddHex(HexController hex, Vector2Int index) {
        _hexes[index] = hex;
    }

    public HexController GetHex(Vector2Int index) {
        return HasHex(index) ? _hexes[index] : null;
    }

    public void RemoveHex(Vector2Int index) {
        _hexes.Remove(index);
    }

    public bool HasHex(Vector2Int index) {
        return _hexes.ContainsKey(index);
    }
}

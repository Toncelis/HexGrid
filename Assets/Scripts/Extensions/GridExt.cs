using System;
using UnityEngine;

public static class GridExt {
    public const float HEX_SIZE = 1f;
    public static float HEX_INNER_RADIUS => HEX_SIZE * Mathf.Sqrt(3) / 2;
    
    public static Vector3 GetPosition(Vector2Int index) {
        int columnModifier = Mathf.Abs(index.x % 2);
        return (2*index.y+columnModifier) * HEX_INNER_RADIUS * Vector3.forward + (1.5f*index.x)*HEX_SIZE*Vector3.right;
    }

    public static Vector2Int[] NeighborDirections(Vector2Int index) => new Vector2Int[] {
        up,
        RightUp(index),
        RightDown(index),
        down,
        LeftDown(index),
        LeftUp(index)
    };

    public static Vector2Int up => Vector2Int.up;
    public static Vector2Int down => Vector2Int.down;

    public static Vector2Int RightUp(Vector2Int index) => Vector2Int.right + Vector2Int.up * Mathf.Abs(index.x % 2); 
    public static Vector2Int RightDown(Vector2Int index) => Vector2Int.right + Vector2Int.down * (1 - Mathf.Abs(index.x % 2));

    public static Vector2Int LeftUp(Vector2Int index) => Vector2Int.left + Vector2Int.up * Mathf.Abs(index.x % 2); 
    public static Vector2Int LeftDown(Vector2Int index) => Vector2Int.left + Vector2Int.down * (1 - Mathf.Abs(index.x % 2));

    public static HexController GetHex(this GridController grid, HexController hex, GridDirection direction) {
        return grid.GetHex(GetIndex(hex.Model.index, direction));
    }
    
    public static Vector2Int GetIndex(Vector2Int index, GridDirection direction) {
        switch (direction) {
            case GridDirection.Up:
                return NeighborDirections(index)[0] + index;
            case GridDirection.RightUp:
                return NeighborDirections(index)[1] + index;
            case GridDirection.RightDown:
                return NeighborDirections(index)[2] + index;
            case GridDirection.Down:
                return NeighborDirections(index)[3] + index;
            case GridDirection.LeftDown:
                return NeighborDirections(index)[4] + index;
            case GridDirection.LeftUp:
                return NeighborDirections(index)[5] + index;
        }
        throw new NotImplementedException();
    }
}
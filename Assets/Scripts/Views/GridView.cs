using UnityEngine;

public class GridView : MonoBehaviour {
    public GridSettings Settings;
    public int GridWidth, GridLength;

    private GridController _gridController;
    
    public void Start() {
        _gridController = new GridController(Settings, GridWidth, GridLength, transform);
    }

    public void OnLeftHold(Vector2Int hexIndex) {
        _gridController.SelectHex(hexIndex);
    }
    public void OnLeftClick(Vector2Int hexIndex) {
        _gridController.OnLeftClick(hexIndex);
    }
    public void OnLeftClick(CharController character) {
        _gridController.OnLeftClick(character);
    }
    public void OnShiftLeftClick(Vector2Int hexIndex) {
        _gridController.SurroundWithHexes(hexIndex);
    }
    public void OnCtrlLeftClick(Vector2Int hexIndex) {
        _gridController.RemoveHex(hexIndex);
    }

    public void OnRightClick() {
        _gridController.OnRightClick();
    }
}
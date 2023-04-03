using UnityEngine;

public class GridView : MonoBehaviour {
    public GridSettings Settings;
    public int GridWidth, GridLength;

    private GridController _gridController;
    public GameFlow Flow;
    
    public void Start() {
        _gridController = new GridController(Settings, GridWidth, GridLength, transform, this);
    }

    public void StartGame() {
        Flow.ChangeState(new CharacterChoosingState(Flow, _gridController));
    }

    public void OnLeftHold(Vector2Int hexIndex) {
        _gridController.SelectHex(hexIndex);
    }
    public void OnShiftLeftClick(Vector2Int hexIndex) {
        _gridController.SurroundWithHexes(hexIndex);
    }
    public void OnCtrlLeftClick(Vector2Int hexIndex) {
        _gridController.RemoveHex(hexIndex);
    }
}
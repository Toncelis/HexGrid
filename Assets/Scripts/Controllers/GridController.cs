using UnityEngine;

public class GridController {
    private readonly Grid _grid;
    public Grid model => _grid;

    private Vector2Int _selectionIndex = Vector2Int.zero;
    private bool _hasSelection = false;
    private readonly Transform _gridHolder;

    private bool _characterSelected = false;
    private CharController _char;

    public GridController(GridSettings settings, int width, int length, Transform gridHolder) {
        _grid = new Grid(settings);
        SetupGrid(width, length);
        _gridHolder = gridHolder;
    }

    public void SurroundWithHexes(Vector2Int index) {
        foreach (var direction in GridExt.NeighborDirections(index)) {
            if (!model.HasHex(index + direction)) {
                AddHex(index + direction, Random.Range(-0.2f,0.2f));
            }
        }
    }

    public void RemoveHex(Vector2Int index) {
        model.GetHex(index)?.Destroy();
        model.RemoveHex(index);
        if (_selectionIndex == index) {
            _hasSelection = false;
        }
    }

    private void SetupGrid(int width, int length) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < length; y++) {
                var index = new Vector2Int(x, y);
                AddHexInstant(index, Random.Range(-0.2f, 0.2f));
            }
        }
    }

    private void AddHexInstant(Vector2Int index, float height = 0) {
        var hex = new HexController(index, height, HexType.standard);
        _grid.AddHex(hex, index);
        hex.InstantiateHexImmediate(_gridHolder);
    }
    private void AddHex(Vector2Int index, float height = 0) {
        var hex = new HexController(index, height, HexType.standard);
        _grid.AddHex(hex, index);
        hex.InstantiateHex(_gridHolder);
    }

    public HexController GetHex(Vector2Int index) {
        return _grid.GetHex(index);
    }

    public void SelectHex(Vector2Int index) {
        if (_hasSelection) {
            if (_selectionIndex == index) {
                return;
            }
            GetHex(_selectionIndex)?.ClearMarks();
        }

        _hasSelection = true;
        _selectionIndex = index;
        GetHex(_selectionIndex)?.Select();
    }

    public void OnLeftClick(Vector2Int index) {
        if (!_grid.HasHex(index)) {
            return;
        }

        var hex = _grid.GetHex(index);

        if (_characterSelected) {
            switch (hex.Model.state) {
                case HexStateType.Empty:
                    MoveCharacter(hex);
                    break;
                case HexStateType.Occupied:
                    SelectCharacter(hex.Model.occupant);
                    break;
            }
        } else {
            switch (hex.Model.state) {
                case HexStateType.Empty:
                    PlaceCharacter(hex);
                    break;
                case HexStateType.Occupied:
                    SelectCharacter(hex.Model.occupant);
                    break;
            }
        }
    }

    public void OnLeftClick(CharController character) {
        SelectCharacter(character);
    }

    public void OnRightClick() {
        ClearHexSelection();
        FreeCharacter();
    }

    private CharController PlaceCharacter(HexController hex) {
        CharController character = new CharController(hex);
        hex.Occupy(character);
        SelectCharacter(character);
        return character;
    }

    private void SelectCharacter(CharController character) {
        FreeCharacter();

        _characterSelected = true;
        _char = character;
        _char.Mark();
        ClearHexSelection();
    }

    private void MoveCharacter(HexController hex) {
        if (!_characterSelected) {
            return;
        }

        _char.MoveTo(hex);
        FreeCharacter();
        ClearHexSelection();
    }

    private void FreeCharacter() {
        if (_characterSelected) {
            _characterSelected = false;
            _char.ClearMark();
        }
    }

    private void ClearHexSelection() {
        if (_hasSelection) {
            _hasSelection = false;
            GetHex(_selectionIndex).ClearMarks();
        }
    }
}
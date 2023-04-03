using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;  

public class GridController {
    private readonly Grid _grid;
    public Grid model => _grid;

    private readonly GridView _view;

    private Vector2Int _selectionIndex = Vector2Int.zero;
    private bool _hasSelection = false;
    private readonly Transform _gridHolder;

    private bool _characterSelected = false;
    private CharController _char;

    private TileLibrary _tileLibrary;

    public GridController(GridSettings settings, int width, int length, Transform gridHolder, GridView view) {
        _grid = new Grid(settings);
        _gridHolder = gridHolder;
        _view = view;
        var libraryLoading = TileLibrary.LoadAsync();
        libraryLoading.Completed += (handle) => {
            _tileLibrary = handle.Result;
            SetupGrid(width, length);
            _view.StartGame();
        };
    }

    public void SurroundWithHexes(Vector2Int index) {
        foreach (var direction in GridExt.NeighborDirections(index)) {
            if (!model.HasHex(index + direction)) {
                AddHexOfRandomType(index + direction);
            }
        }
        
        ClearHexSelection();
    }

    public void RemoveHex(Vector2Int index) {
        model.GetHex(index)?.Destroy();
        model.RemoveHex(index);
        if (_selectionIndex == index) {
            _hasSelection = false;
        }
        
        ClearHexSelection();
    }

    private void SetupGrid(int width, int length) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < length; y++) {
                var index = new Vector2Int(x, y);
                AddHexOfRandomType(index);
            }
        }
    }

    private void AddHexOfRandomType(Vector2Int index) => AddHex(index, Enum<HexType>.Random());
    private void AddHexOfRandomTypeInstant(Vector2Int index) => AddHexInstant(index, Enum<HexType>.Random());
    
    private void AddHexInstant(Vector2Int index, HexType type) {
        var hex = new HexController(index, _tileLibrary.GetInfo(type));
        _grid.AddHex(hex, index);
        hex.InstantiateHexImmediate(_gridHolder);
    }
    
    private void AddHex(Vector2Int index, HexType type) {
        var hex = new HexController(index, _tileLibrary.GetInfo(type));
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

    public CharController PlaceCharacter(HexController hex) {
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

    public Dictionary<HexController, Path> GetReachableHexes(HexController startHex, int speed) {
        Dictionary<HexController, Path> result = new() {
            { startHex, new Path(new List<HexController>(), speed) }
        };
        List<HexController> edgeTiles = new() { startHex };
        
        while (edgeTiles.Any()) {
            var newReaches = new List<HexController>();
            foreach (var hex in edgeTiles) {
                foreach (var direction in (GridDirection[])Enum.GetValues(typeof(GridDirection))) {
                    var nextHex = this.GetHex(hex, direction);
                    if (nextHex == null) {
                        continue;
                    }

                    var moveCost = hex.Model.MovementCost(nextHex.Model.type);
                    if (moveCost < 0) {
                        continue;
                    }
                    
                    int speedLeft = result[hex].speedLeft - moveCost;

                    if (speedLeft < 0 || nextHex.Model.state != HexStateType.Empty) {
                        continue;
                    }

                    if (result.ContainsKey(nextHex) && result[nextHex].speedLeft >= speedLeft) {
                        continue;
                    }
                    
                    newReaches.AddIfNewAndNotNull(nextHex);
                    var steps = result[hex].steps;
                    steps.Add(nextHex);
                    result[nextHex] = new Path(steps, speedLeft);
                }
            }
            
            edgeTiles = newReaches;
        }

        result.Remove(startHex);
        return result;
    }

    public HexController GetRandomHex(HexStateType state) {
        return model.GetRandomHex(state);
    }
}
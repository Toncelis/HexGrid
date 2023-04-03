using System.Collections.Generic;
using System.Linq;

public class MovementPlanningState : FlowState {
    private CharController _character;
    private Dictionary<HexController, Path> _movementMap;

    public MovementPlanningState(GameFlow flowController, GridController gridController, CharController character)
        : base(flowController, gridController) {
        _character = character;
    }

    public override void EnterState() {
        _movementMap = grid.GetReachableHexes(_character.hex, Char.SPEED);
        if (!_movementMap.Any()) {
            flow.ChangeState(new CharacterChoosingState(flow, grid));
            return;
        }
        
        _character.Mark();
        foreach (var hex in _movementMap.Keys) {
            hex.ShowAsAvailable(Char.SPEED - _movementMap[hex].speedLeft);
        }
    }

    public override void OnTileClick(HexController hex) {
        if (hex == _character.hex) {
            flow.ChangeState(new CharacterChoosingState(flow, grid));
            return;
        }

        if (_movementMap.Keys.Contains(hex)) {
            var start = _character.hex;
            foreach (var step in _movementMap[hex].steps) {
                start.PointTo(step);
                start = step;
            }
            _character.MoveTo(hex);
            
            var _newMap = grid.GetReachableHexes(hex, _movementMap[hex].speedLeft);
            
            foreach (var reachableHex in _movementMap.Keys) {
                reachableHex.Refresh();
            }
            foreach (var reachableHex in _newMap.Keys) {
                reachableHex.ShowAsAvailable(Char.SPEED - _newMap[hex].speedLeft);
            }

            if (_movementMap[hex].speedLeft > 0 && _newMap.Any()) {
                _movementMap = _newMap;
            } else {
                flow.ChangeState(new CharacterChoosingState(flow, grid));
            }
        }
    }

    public override void OnRightClick() {
        flow.ChangeState(new CharacterChoosingState(flow, grid));
    }

    public override void ExitState() {
        foreach (var reachableHex in _movementMap.Keys) {
            reachableHex.Refresh();
        }
        _character.ClearMark();
    }
}

public class Path {
    private List<HexController> _steps;
    private int _speedLeft;

    public List<HexController> steps => _steps.ToList();
    public int speedLeft => _speedLeft;

    public Path(List<HexController> steps, int speedLeft) {
        _steps = steps.ToList();
        _speedLeft = speedLeft;
    }
}

public enum GridDirection {
    Up,
    RightUp,
    RightDown,
    Down,
    LeftDown,
    LeftUp
}
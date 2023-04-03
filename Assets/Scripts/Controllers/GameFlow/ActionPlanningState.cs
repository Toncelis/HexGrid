using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterChoosingState : FlowState {
    public CharacterChoosingState(GameFlow flow, GridController grid) : base(flow, grid) {}
    public override void EnterState() {
        HexController hex = grid.GetRandomHex(HexStateType.Empty);
        grid.PlaceCharacter(hex);
    }

    public override void OnTileClick(HexController hex) {
        if (hex.Model.state == HexStateType.Occupied) {
            flow.ChangeState(new MovementPlanningState(flow, grid, hex.Model.occupant));
        }
    }

    public override void OnCharClick(CharController character) {
        flow.ChangeState(new MovementPlanningState(flow, grid, character));
    }
}

/*
public class TurnInfo {
    private int _abilityIndex;
    private bool _abilityQueued = false;
    private List<Vector2Int> _movement = new();
}

public abstract class AbilityAimData {}

public class DirectionAbilityAimData : AbilityAimData {
    public DirectionAbilityAimData(float angle) {
        _angle = angle;
    }
    
    private readonly float _angle;
    public float angle => _angle;
}

public class TargetAbilityAimData : AbilityAimData {
    public TargetAbilityAimData(List<Vector2Int> targetIndexes) {
        _targetIndexes = targetIndexes.ToArray();
    }

    private readonly Vector2Int[] _targetIndexes;
    public Vector2Int[] targetIndexes => _targetIndexes.ToArray();
}
*/
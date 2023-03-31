using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour {
    private FlowState _currentState;

    public void ChangeState(FlowState state) {
        if (_currentState == state) {
            return;
        }

        _currentState.ExitState();
        _currentState = state;
        _currentState.EnterState();
    }

    public void OnTileClick(Vector2Int index) {
        _currentState.OnTileClick(index);
    }

    public void OnRightClick() {
        _currentState.OnRightClick();
    }

    public void OnAbilityClick() {
        _currentState.OnAbilityClick();
    }
}

public abstract class FlowState {
    public GridController grid;
    public GameFlow flow;
    
    public virtual void OnTileClick(Vector2Int index) {
        
    }

    public virtual void OnConfirmClick() {
        
    }

    public virtual void OnRightClick() {
        
    }

    public virtual void OnAbilityClick() {
        
    }

    public virtual void ExitState() {
        
    }

    public virtual void EnterState() {
        
    }
};

public class ChooseCharacterState : FlowState {
    public override void OnTileClick(Vector2Int index) {
        var hex = grid.GetHex(index);
    }
}

public class ChoosePathState : FlowState {
    private readonly List<Vector2Int> path = new List<Vector2Int>();

    public override void OnRightClick() {
        if (path.Count > 0) {
            path.Clear();
        } else {
            flow.ChangeState(new ChooseCharacterState());
        }
    }
}

public class AimState : FlowState {
}

public class ResolvingState : FlowState {
    
}
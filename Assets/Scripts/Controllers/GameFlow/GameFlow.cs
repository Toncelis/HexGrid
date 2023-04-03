using UnityEngine;

public class GameFlow : MonoBehaviour {
    private FlowState _currentState;

    public void ChangeState(FlowState state) {
        if (_currentState == state) {
            return;
        }

        _currentState?.ExitState();
        _currentState = state;
        _currentState.EnterState();
    }

    public void OnTileClick(HexController hex) {
        _currentState.OnTileClick(hex);
    }

    public void OnCharClick(CharController character) {
        _currentState.OnCharClick(character);
    }

    public void OnRightClick() {
        _currentState.OnRightClick();
    }

    public void OnAbilityClick() {
        _currentState.OnAbilityClick();
    }
}
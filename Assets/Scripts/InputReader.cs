using UnityEngine;

public class InputReader : MonoBehaviour {
    [SerializeField] private Camera Camera;
    [SerializeField] private GridView GridView;
    [SerializeField] private GameFlow GameFlow;

    private float _leftClickTime;
    private ClickType _clickType = ClickType.Click;
    
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                _clickType = ClickType.ShiftClick;
            } else if (Input.GetKey(KeyCode.LeftControl)) {
                _clickType = ClickType.CtrlClick;
            } else {
                _clickType = ClickType.Click;
            }
            _leftClickTime = 0f;
        }
        
        if (Input.GetMouseButton(0)) {
            OnLeftHold();
            _leftClickTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0)) {
            switch (_clickType) {
                case ClickType.Click:
                    OnLeftClick();
                    break;
                case ClickType.ShiftClick:
                    if (Input.GetKey(KeyCode.LeftShift)) {
                        OnShiftLeftClick();
                    } else {
                        OnLeftClick();
                    }
                    break;
                case ClickType.CtrlClick:
                    if (Input.GetKey(KeyCode.LeftControl)) {
                        OnCtrlLeftClick();
                    } else {
                        OnLeftClick();
                    }
                    break;
                case ClickType.RightClick:
                    break;
            }
            _leftClickTime = 0;
        }

        if (Input.GetMouseButtonUp(1)) {
            OnRightClick();
        }
    }

    
    private void OnLeftHold() {
        if (TryGetHexIndexUnderMouse(out var hex)) {
            GridView.OnLeftHold(hex.Model.index);
        }
    }

    private void OnLeftClick() {
        if (TryGetHexIndexUnderMouse(out var hex)) {
            GameFlow.OnTileClick(hex);
        } else if  (TryGetCharUnderMouse(out var character)) {
            GameFlow.OnCharClick(character);
        }
    }
    private void OnShiftLeftClick() {
        if (TryGetHexIndexUnderMouse(out var hex)) {
            GridView.OnShiftLeftClick(hex.Model.index);
        }
    }
    private void OnCtrlLeftClick() {
        if (TryGetHexIndexUnderMouse(out var hex)) {
            GridView.OnCtrlLeftClick(hex.Model.index);
        }
    }

    private void OnRightClick() {
        GameFlow.OnRightClick();
    }

    private bool TryGetHexIndexUnderMouse(out HexController hex) {
        var isHit = Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out var hit);
        if (!isHit) {
            hex = null;
            return false;
        }

        if (!hit.transform.parent.gameObject.TryGetComponent<HexView>(out var view)) {
            hex = null;
            return false;
        }

        hex = view.controller;
        return true;
    }

    private bool TryGetCharUnderMouse(out CharController character) {
        var isHit = Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out var hit);
        if (!isHit || !hit.transform.parent.gameObject.TryGetComponent<CharView>(out var charView)) {
            character = null;
            return false;
        }

        character = charView.controller;
        return true;
    }
}

public enum ClickType {
    Click,
    ShiftClick,
    CtrlClick,
    RightClick
}
using UnityEngine;

public class InputReader : MonoBehaviour {
    [SerializeField] private Camera Camera;
    [SerializeField] private GridView GridView;

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
        if (TryGetHexIndexUnderMouse(out var index)) {
            GridView.OnLeftHold(index);
        }
    }

    private void OnLeftClick() {
        if (TryGetHexIndexUnderMouse(out var index)) {
            GridView.OnLeftClick(index);
        } else if  (TryGetCharUnderMouse(out var character)) {
            GridView.OnLeftClick(character);
        }
    }
    private void OnShiftLeftClick() {
        if (TryGetHexIndexUnderMouse(out var index)) {
            GridView.OnShiftLeftClick(index);
        }
    }
    private void OnCtrlLeftClick() {
        if (TryGetHexIndexUnderMouse(out var index)) {
            GridView.OnCtrlLeftClick(index);
        }
    }

    private void OnRightClick() {
        GridView.OnRightClick();
    }

    private bool TryGetHexIndexUnderMouse(out Vector2Int index) {
        var isHit = Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out var hit);
        if (!isHit) {
            index = Vector2Int.zero;
            return false;
        }

        if (!hit.transform.parent.gameObject.TryGetComponent<HexView>(out var hex)) {
            index = Vector2Int.zero;
            return false;
        }

        index = hex.index;
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
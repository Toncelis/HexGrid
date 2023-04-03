using System;
using UnityEngine.AddressableAssets;

public class CharController {
    private HexController _myHex;
    private CharView _view;
    private Char _model;

    public Char model => _model;

    public CharController(HexController hex) {
        string refAddress;
        switch (hex.Model.type) {
            case HexType.Grass:
            case HexType.Ground:
            case HexType.HighGround:
                refAddress = "Assets/Prefabs/Character.prefab";
                break;
            case HexType.ShallowWater:
            case HexType.DeepWater:
                refAddress = "Assets/Prefabs/WaterCharacter.prefab";
                break;
            default:
                throw new NotImplementedException();
        }
        
        var handle = Addressables.InstantiateAsync(refAddress);
        _myHex = hex;

        handle.Completed += (handle) => {
            _view = handle.Result.GetComponent<CharView>();
            _view.Setup(this, _myHex);
        };

        _model = new Char(_myHex);
        _myHex.Occupy(this);
    }

    public void MoveTo(HexController newHex) {
        _myHex.Free();
        _view.MoveTo(newHex);
        newHex.Occupy(this);
        _model.SetHex(newHex);
        _myHex = newHex;
    }

    public void Mark() {
        if (_view) {
            _view.Mark();
        }
    }

    public void ClearMark() {
        if (_view) {
            _view.ClearMark();
        }
    }

    public HexController hex => _myHex;
}
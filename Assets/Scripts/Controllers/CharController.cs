using UnityEngine.AddressableAssets;

public class CharController {
    private HexController _myHex;
    private CharView _view;
    private Char _model;
    
    public CharController(HexController hex) {
        var handle = Addressables.InstantiateAsync("Assets/Prefabs/Character.prefab");
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
        var handle = Addressables.InstantiateAsync("Assets/Prefabs/Arrow.prefab");
        var from = _myHex.position;
        var to = newHex.position;
        handle.Completed += (handle) => {
            handle.Result.GetComponent<ArrowView>().Setup(from, to);
        };
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
}
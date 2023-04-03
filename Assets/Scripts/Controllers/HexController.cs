using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class HexController {
    private const string STANDARD_TILE_KEY = "Assets/Prefabs/GrassTile.prefab";
    
    private Hex _model;
    private HexView _view;

    private bool _setupComplete = false;
    private Action _onSetupComplete = () => {};

    public Hex Model => _model;

    public HexController(Vector2Int index, TileInfo info) {
        _model = new Hex(index, info.height, info);
    }

    public AsyncOperationHandle<GameObject> InstantiateHex(Transform gridHolder) {
        var handle = StartHexInstantiation(gridHolder);
        handle.Completed += (handle) => _view.PlayAppearanceAnimation();
        return handle;
    }

    public AsyncOperationHandle<GameObject> InstantiateHexImmediate(Transform gridHolder) {
        return StartHexInstantiation(gridHolder);
    }

    private AsyncOperationHandle<GameObject> StartHexInstantiation(Transform gridHolder) {
        var hexHandle = Addressables.LoadAssetAsync<GameObject>(_model.reference);
        hexHandle.Completed +=  (hexHandle) => {
            OnHexLoadComplete(hexHandle, gridHolder);
            _setupComplete = true;
            _onSetupComplete();
        };
        return hexHandle;

    }

    private void OnHexLoadComplete(AsyncOperationHandle<GameObject> handle, Transform gridHolder) {
        var hexGameObject = Object.Instantiate(handle.Result, gridHolder);
        _view = hexGameObject.GetComponent<HexView>();
        _view.Setup(this, HexType.Grass);
    }

    public void ClearMarks() {
        _view.ClearMark();
    }

    public void Select() {
        _view.SetSelected();
    }

    public void Destroy() {
        _view.Destroy();
    }

    public void DestroyImmediate() {
        _view.DestroyImmediate();
    }

    public void Occupy(CharController character) {
        _model.Occupy(character);
        Refresh();
    }
    public void Free() {
        _model.Free();
        Refresh();
    }

    public void ShowAsAvailable(int movementCost) {
        _view.ShowAsAvailable(movementCost);
    }
    
    public void Refresh() {
        if (!_setupComplete) {
            _onSetupComplete += Refresh;
            return;
        }
        _view.Refresh();
    }

    public void PointTo(HexController targetHex) {
        var handle = Addressables.InstantiateAsync("Assets/Prefabs/Arrow.prefab");
        handle.Completed += (handle) => {
            handle.Result.GetComponent<ArrowView>().Setup(this.position, targetHex.position);
        };
    }

    public Vector3 position => GridExt.GetPosition(Model.index) + Vector3.up * Model.height;
}
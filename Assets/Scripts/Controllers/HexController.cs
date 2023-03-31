using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class HexController {
    private const string STANDARD_TILE_KEY = "Assets/Prefabs/GrassTile.prefab";
    
    private Hex _model;
    private HexView _view;

    public Hex Model => _model;

    public HexController(Vector2Int index, float height, HexType type) {
        _model = new Hex(index, height, type);
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
        var hexHandle = Addressables.LoadAssetAsync<GameObject>(STANDARD_TILE_KEY);
        hexHandle.Completed +=  (hexHandle) => OnHexLoadComplete(hexHandle, gridHolder);
        return hexHandle;

    }

    private void OnHexLoadComplete(AsyncOperationHandle<GameObject> handle, Transform gridHolder) {
        var hexGameObject = Object.Instantiate(handle.Result, gridHolder);
        _view = hexGameObject.GetComponent<HexView>();
        _view.Setup(this, HexType.standard);
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
        _view.Occupy();
    }
    public void Free() {
        _model.Free();
        _view.Free();
    }

    public Vector3 position => GridExt.GetPosition(Model.index) + Vector3.up * Model.height;
}
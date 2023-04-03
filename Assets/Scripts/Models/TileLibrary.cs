using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Libraries/TileInfoByType", fileName = "TileInfoLibrary")]
public class TileLibrary : ScriptableObject {
    [SerializeField] private TileInfo[] TileInfoLib;
    public TileInfo GetInfo(HexType type) {
        return TileInfoLib[(int)type];
    }

    public static AsyncOperationHandle<TileLibrary> LoadAsync() {
        return Addressables.LoadAssetAsync<TileLibrary>("Assets/Settings/TileInfoLibrary.asset");
    }
}

[Serializable]
public class TileInfo {
    [SerializeField] private HexType _type;
    [ListDrawerSettings(OnBeginListElementGUI = nameof(TypeIndexToNameGUIStart),OnEndListElementGUI = nameof(TypeIndexToNameGUIEnd), Expanded = true, DraggableItems = false)]
    [SerializeField] private int[] _movementCostByType = new int[Enum.GetNames(typeof(HexType)).Length];
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;
    [SerializeField] private AssetReference _hexReference;
    
    
    public HexType type => _type;
    public int MovementCostTo(HexType type) => _movementCostByType[(int)type];
    public float height => Random.Range(_minHeight, _maxHeight);
    public  AssetReference hexReference => _hexReference;

    #if UNITY_EDITOR
    private void TypeIndexToNameGUIStart(int index) {
        string name;
        string[] typeNames = Enum.GetNames(typeof(HexType));
        if (index >= typeNames.Length) {
            name = "Unidentified";
        } else {
            name = typeNames[index];
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label(name,  GUILayoutOptions.Width(90));
    }

    private void TypeIndexToNameGUIEnd(int index) {
        GUILayout.EndHorizontal();
    }
    #endif
}

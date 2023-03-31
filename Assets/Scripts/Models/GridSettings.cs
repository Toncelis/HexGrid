using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "gridSettings", menuName = "Settings/Grid")]
public class GridSettings : ScriptableObject {
    [SerializeField, VerticalGroup("B")]
    private float HexSize;
    public float hexSize => HexSize;
    public float hexInnerRadius => HexSize * Mathf.Sqrt(3) / 2;

}
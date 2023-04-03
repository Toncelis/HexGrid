using UnityEngine;
using UnityEngine.AddressableAssets;

public class Hex {

    private readonly TileInfo _info;
    public HexStateType state { get; private set; }
    public CharController occupant { get; private set; }
    public Vector2Int index { get; }
    public float height { get; }
    
    
    public HexType type => _info.type;
    public AssetReference reference => _info.hexReference;
    public int MovementCost(HexType destinationType) => _info.MovementCostTo(destinationType); 

    public Hex(Vector2Int index, float height, TileInfo info) {
        this.index = index;
        this.height = height;
        _info = info;
        occupant = null;
    }

    public void Occupy(CharController occupant) {
        this.occupant = occupant;
        state = HexStateType.Occupied;
    }

    public void Free() {
        occupant = null;
        state = HexStateType.Empty;
    }
}
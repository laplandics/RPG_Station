using UnityEngine;

[CreateAssetMenu(fileName = "AllTilesSettings", menuName = "GameSettings/Tiles/AllTilesSettings")]
public class AllTilesMajorSettingsSo : ScriptableObject, IMajorSettings
{
    [SerializeField] private string instanceKey;
    public TileSettingsSo[] tilesSettings;
    
    public string InstanceKey { get => instanceKey; set => instanceKey = value; }
    
    public bool TrySet(IDataHandler handler) => handler is TilesDataHandler && handler.TrySetSettings(this);
}
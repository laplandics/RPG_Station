using UnityEngine;

[CreateAssetMenu(fileName = "AllTerrainsSettings", menuName = "GameSettings/Terrain/AllTerrainsSettings")]
public class AllTerrainTypesMajorSettingsSo : ScriptableObject, IMajorSettings
{
    public string instanceKey;
    public string InstanceKey { get => instanceKey; set => instanceKey = value; }
    
    public TerrainDataGenerator[] terrainDataGenerators;

    public bool TrySet(IDataHandler handler) => handler is TerrainDataHandler && handler.TrySetSettings(this);
}
using UnityEngine;

[CreateAssetMenu(fileName = "AllBiomesSettings", menuName = "GameSettings/Biomes/AllBiomesSettings")]
public class AllBiomesMajorSettingsSo : ScriptableObject, IMajorSettings
{
    public string instanceKey;
    public string InstanceKey { get => instanceKey; set => instanceKey = value; }
    
    [Header("Biome Settings")]
    public BiomeSettingsSo[] biomeSettings;
    public int BiomesCount => biomeSettings.Length - 1;

    [Header("VoronoiNoise Settings")]
    public int seed;
    public float borderSize;
    public float jitter;
    public int biomeSize;   
    
    public bool TrySet(IDataHandler handler) => handler is BiomesDataHandler && handler.TrySetSettings(this);
}
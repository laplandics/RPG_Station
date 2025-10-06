using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeSettings", menuName = "GameSettings/Biomes/BiomeSettings")]
public class BiomeSettingsSo : ScriptableObject
{
    public string biomeKey;
    public BiomesType biomeType;
    public float biomeWeight;
    public TileSettingsSo[] biomeTiles;
    public ScriptableObject[] terrainGenerationPresets;

    [Button]
    private void Check()
    {
        if (terrainGenerationPresets.Length == 0) { Debug.LogWarning($"No terrain generation presets found for {biomeKey}"); }
        for (var i = 0; i < terrainGenerationPresets.Length; i++)
        {
            var preset = terrainGenerationPresets[i];
            if (preset != null) continue;
            Debug.LogWarning($"Terrain generation preset on {i} is missing");
        }
        var tileValidator = new BiomeTilesInfoValidator();
        tileValidator.ValidateTiles(biomeTiles, this);
    }
}
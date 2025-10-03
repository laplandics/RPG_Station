using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainSettingsSo", menuName = "GameSettings/Terrain/TerrainSettingsSo")]
public class TerrainSettingsSo : ScriptableObject
{
    public string terrainKey;

    [Header("Terrain Data")]
    public BiomeTypePairs.BiomeType biomeType;
    [MinMaxSlider(0.0f, 1.0f)]public Vector2 noise;
    
    [Header("Enemies Data")]
    [Range(0.0f, 10.0f)]public float enemySpawnMult;
    public List<Enemy> allowedEnemies;

    private void OnValidate()
    {
        var noiseX = Mathf.Round(noise.x * 100f) / 100f;
        var noiseY = Mathf.Round(noise.y * 100f) / 100f;
        noise = new Vector2(noiseX, noiseY);
    }
}
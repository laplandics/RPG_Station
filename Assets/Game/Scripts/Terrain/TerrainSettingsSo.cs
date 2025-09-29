using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainSettingsSo", menuName = "GameSettings/Terrain/TerrainSettingsSo")]
public class TerrainSettingsSo : ScriptableObject
{
    public string terrainKey;

    [Header("Terrain Data")]
    public Chunk chunkPrefab;
    public TerrainType terrainType;
    public float noiseMin;
    public float noiseMax;
    
    [Header("Enemies Data")]
    [Range(0.0f, 10.0f)]public float enemySpawnMult;
    public List<Enemy> allowedEnemies;
}
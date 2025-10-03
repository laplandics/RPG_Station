using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string instanceKey;
}

[Serializable]
public class PlayerData : SaveData
{
    public int x;
    public int y;
    public int animationSpeed;
    public float baseStepsDelay;
    public float minStepsDelay;
    public float decreaseRate;
}

[Serializable]
public class MapData : SaveData
{
    public int chunkSize;
    public int renderChunksCount;
    public int memorizedArea;
    public int tilesCalculationArea;
    public int seed;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public int atlasColumns;
    public int atlasRows;
    public Queue<Vector2Int> ChunksToRemember = new();
}

[Serializable]
public class AllTerrainsData : SaveData
{
    public List<TerrainData> allTerrainsData;
}

[Serializable]
public class TerrainData : SaveData
{
    public BiomeTypePairs.BiomeType biomeType;
    public Vector2 noise;
    public float enemySpawnMult;
    public List<string> allowedEnemiesPrefabKeys;
}

[Serializable]
public class EnemiesData : SaveData
{
    public int enemiesGenerationDistance;
    public int enemiesGenerationArea;
    public float averageGenerationEnemiesChance;
    public Vector2Int[] recentChunks;
    public List<EnemyData> enemies;
}

[Serializable]
public class EnemyData : SaveData
{
    public int x;
    public int y;
    public bool isSpawned;
}

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
}

[Serializable]
public class MapData : SaveData
{
    public int chunkSize;
    public int mapSize;
    public int renderAreaSize;
    public int seed;
    public float scale;
    public float multX;
    public float multY;
    public int threshold;
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

[Serializable]
public class TerrainsData : SaveData
{
    public Dictionary<TerrainType, TerrainData> AllTerrainsData;
}

[Serializable]
public class TerrainData : SaveData
{
    public TerrainType terrainType;
    public float noiseMin;
    public float noiseMax;
    public float enemySpawnMult;
    public List<string> allowedEnemiesPrefabKeys;
}
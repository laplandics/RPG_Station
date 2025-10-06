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
    public int atlasColumns;
    public int atlasRows;
    public Queue<Vector2Int> ChunksToRemember = new();
}

[Serializable]
public class AllTilesData : SaveData
{
    public List<TileData> allTilesData;
}

[Serializable]
public class TileData : SaveData
{
    public Vector2 noise;
    public int tileAtlasIndex;
    public bool isUnreachable;
}

[Serializable]
public class AllBiomesData : SaveData
{
    public int biomesCount;
    public int seed;
    public float borderSize;
    public float jitter;
    public int biomeSize;
    public List<BiomeData> allBiomesData;
}

[Serializable]
public class BiomeData : SaveData
{
    public BiomesType biomeType;
    public float weight;
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

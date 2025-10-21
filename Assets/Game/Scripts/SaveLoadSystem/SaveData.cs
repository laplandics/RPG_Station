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
    public uint seed;
    public int chunkSize;
    public int renderChunksCount;
    public int memorizedArea;
    public int tilesCalculationArea;
    public int visibleChunks;
    public TerrainType defaultTerrainType;
    public Queue<Vector2Int> ChunksToRemember = new();
    public int columns;
    public int rows;
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
    public bool isUnreachable;
}

[Serializable]
public class AllTerrainData : SaveData
{
    public List<TerrainData> allTerrainData;
}

[Serializable]
public class TerrainData : SaveData
{
    public float weight;
}
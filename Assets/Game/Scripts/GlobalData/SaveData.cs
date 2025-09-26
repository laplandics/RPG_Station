using System;

[Serializable]
public class SaveData
{
    public string instanceKey;
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
public class PlayerData : SaveData
{
    public int x;
    public int y;
}
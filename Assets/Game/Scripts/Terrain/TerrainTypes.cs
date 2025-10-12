using System.Collections.Generic;

public enum TerrainType
{
    Void,
    Rock,
}

public static class TerrainTypePairs
{
    public static readonly Dictionary<TerrainType, float> TerrainTypeDictionary = new()
    {
        [TerrainType.Void] = 0.0f,
        [TerrainType.Rock] = 0.1f,
    };
}
using System;
using System.Collections.Generic;

[Serializable]
public static class BiomeTypePairs
{
    [Serializable]
    public enum BiomeType
    {
        Water,
        Meadow,
        Forest,
        Mountain
    }
    
    public static Dictionary<int, BiomeType> BiomeTypes = new()
    {
        { 0, BiomeType.Water },
        { 1, BiomeType.Meadow },
        { 2, BiomeType.Forest },
        { 3, BiomeType.Mountain },
    };
}
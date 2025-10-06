using System.Collections.Generic;

public enum BiomesType
{
    Border,
    FirstZone,
    SecondZone,
    ThirdZone,
    FourthZone,
}

public static class BiomesIndexPairs
{
    public static readonly Dictionary<int, BiomesType> BiomesIndexes = new()
    {
        [0] = BiomesType.Border,
        [1] = BiomesType.FirstZone,
        [2] = BiomesType.SecondZone,
        [3] = BiomesType.ThirdZone,
        [4] = BiomesType.FourthZone
    };
}
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoiseMap
{
    public static float GetPerlinNoise(int posX, int posY, float scale, int seed, float multX, float multY)
    {
        var noiseX = posX * scale + seed * multX;
        var noiseY = posY * scale + seed * multY;
        var noise = Mathf.PerlinNoise(noiseX, noiseY);

        return noise;
    }

    public static Dictionary<Vector2Int, TerrainType> SmoothTerrain(Dictionary<Vector2Int, TerrainType> initialMap, int threshold, int chunkSize)
    {
        var newMap = new Dictionary<Vector2Int, TerrainType>(initialMap);
        foreach (var kvp in initialMap)
        {
            var pos = kvp.Key;
            var current = kvp.Value;
            var counts = new Dictionary<TerrainType, int>();
            var directions = new Vector2Int[]
            {
                new(-1, -1),
                new(0, -1),
                new(1, -1),
                new(-1, 0),
                new(1, 0),
                new(-1, 1),
                new(0, 1),
                new(1, 1)
            };
            foreach (var dir in directions)
            {
                var neighborPos = pos + dir * chunkSize;
                if (initialMap.TryGetValue(neighborPos, out var neighborType))
                {
                    if (!counts.ContainsKey(neighborType)) counts.TryAdd(neighborType, 0);
                    counts[neighborType]++;
                }
            }
            var majority = current;
            var maxCount = 0;
            foreach (var count in counts) { if (count.Value > maxCount) { maxCount = count.Value; majority = count.Key; } }
            if (majority != current && maxCount >= threshold) newMap[pos] = majority;
        }
        return newMap;
    }
}

using UnityEngine;

public static class BiomesDataGenerator
{
    public static int GetBiomeIndexAt(Vector2Int tilePos, int seed, float borderThicknessFactor, float jitter, int allBiomesCount,
        float[] indexWeights, float voronoiCellSizeInTiles)
    {
        var cellSize = Mathf.Max(0.001f, voronoiCellSizeInTiles);
        var cellCenterPosition = new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f);
        var pixelCellX = Mathf.FloorToInt(cellCenterPosition.x / cellSize);
        var pixelCellY = Mathf.FloorToInt(cellCenterPosition.y / cellSize);
        var nearestPointFirst = float.MaxValue;
        var nearestPointSecond = float.MaxValue;
        var bestCellX = 0;
        var bestCellY = 0;
        for (var ny = -1; ny <= 1; ny++)
        {
            for (var nx = -1; nx <= 1; nx++)
            {
                var cellX = pixelCellX + nx;
                var cellY = pixelCellY + ny;
                var featurePoint = FeaturePointInCell(cellX, cellY, seed, cellSize, jitter);
                var dx = featurePoint.x - cellCenterPosition.x;
                var dy = featurePoint.y - cellCenterPosition.y;
                var distSq = dx * dx + dy * dy;
                if (distSq < nearestPointFirst)
                {
                    nearestPointSecond = nearestPointFirst;
                    nearestPointFirst = distSq;
                    bestCellX = cellX;
                    bestCellY = cellY;
                }
                else if (distSq < nearestPointSecond) { nearestPointSecond = distSq; }
            }
        }
        var f1 = Mathf.Sqrt(nearestPointFirst);
        var f2 = Mathf.Sqrt(nearestPointSecond);
        var threshold = Mathf.Max(borderThicknessFactor, 0.0001f);
        var isBorder = f2 - f1 <= threshold;
        return isBorder ? 0 : CellIndex(bestCellX, bestCellY, seed, allBiomesCount, indexWeights);
    }

    private static Vector2 FeaturePointInCell(int cellX, int cellY, int seed, float cellSize, float jitter)
    {
        var h = (uint)(cellX * 73856093) ^ (uint)(cellY * 19349663) ^ (uint)(seed * 83492791);
        h = (h << 13) ^ h;
        var a = (h * (h * h * 15731u + 789221u) + 1376312589u);
        var rx = ((a & 0xFFFF) / 65536.0f);
        a = (a * 1664525u) + 1013904223u;
        var ry = ((a & 0xFFFF) / 65536.0f);
        var baseX = cellX * cellSize + cellSize * 0.5f;
        var baseY = cellY * cellSize + cellSize * 0.5f;
        var offsetX = (rx - 0.5f) * cellSize * jitter;
        var offsetY = (ry - 0.5f) * cellSize * jitter;
        return new Vector2(baseX + offsetX, baseY + offsetY);
    }

    private static int CellIndex(int cx, int cy, int seed, int numIndices, float[] weights)
    {
        var h = (uint)(cx * 374761393) ^ (uint)(cy * 668265263) ^ (uint)(seed * 2654435761);
        h ^= (h >> 13);
        h *= 1274126177u;
        h ^= (h >> 16);
        var rng = new System.Random((int)h);
        if (weights == null || weights.Length < numIndices) { return rng.Next(1, numIndices + 1); }
        var total = 0f;
        for (var i = 0; i < numIndices; i++) total += weights[i];
        if (total <= 0f) return rng.Next(1, numIndices + 1);
        var r = (float)(rng.NextDouble() * total);
        var cumulative = 0f;
        for (var i = 0; i < numIndices; i++) { cumulative += weights[i]; if (r < cumulative) return i + 1; }
        return numIndices;
    }
}

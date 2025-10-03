using System.Collections.Generic;
using UnityEngine;
using static NoiseGenerator;
using static GlobalMapMethods;
using static TerrainDataHandler;
using static MapDataHandler;

public static class TilesDataGenerator
{
    public static Dictionary<Vector2Int, int> GetChunkTilesIndexes(Vector2Int chunkIndex)
    {
        var tiles = new Dictionary<Vector2Int, int>();
        
        var oct = GetMapData.octaves;
        var per = GetMapData.persistence;
        var lac = GetMapData.lacunarity;
        var scale = GetMapData.scale;
        var seed = GetMapData.seed;
        var mti = GetMapData.atlasColumns * GetMapData.atlasRows;
        
        var random = new System.Random(seed);
        var noiseX = random.Next(-10000, 10000);
        var noiseY = random.Next(-10000, 10000);
        
        var positions = GetTilesPositionsInChunk(chunkIndex);
        foreach (var position in positions)
        {
            var noise = GetFractalNoise(position.x, position.y, oct, per, lac, scale, noiseX, noiseY);
            var tileIndex = GetTileIndex(noise, mti);
            tiles.TryAdd(position, tileIndex);
        }
        
        return tiles;
    }

    private static int GetTileIndex(float noise, int maxIndex)
    {
        foreach (var terrain in GetTerrainSettingsSo)
        {
            if (noise < terrain.noise.x || noise > terrain.noise.y) continue;
            var index = (int)terrain.biomeType;
            return index > maxIndex ? maxIndex : index;
        }
        return 0;
    }
}
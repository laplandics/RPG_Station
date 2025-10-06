using System.Collections.Generic;
using UnityEngine;
using static GameDataInjector;
using static BiomesDataGenerator;
using static BiomesIndexPairs;
using static GlobalMapMethods;

public static class TilesDataGenerator
{
    public static Dictionary<Vector2Int, int> GenerateTiles(Vector2Int chunkIndex)
    {
        var tiles = new Dictionary<Vector2Int, int>();
        var positions = GetTilesPositionsInChunk(chunkIndex);
        var mti = InjectMapData.atlasColumns * InjectMapData.atlasRows;
        
        foreach (var position in positions)
        {
            if(!TryGetBiomeInfo(position, out var preset, out var settings)) continue;
            var noise = preset.GetNoise(position);
            var tileIndex = GetTileIndex(noise, mti, settings);
            tiles.TryAdd(position, tileIndex);
        }
        return tiles;
    }
    
    private static bool TryGetBiomeInfo(Vector2Int tileIndex, out IGenerationPreset preset, out BiomeSettingsSo biomeSettings)
    {
        var bd = InjectBiomesData;
        var weights = new float[bd.biomesCount - 1];
        for (var i = 0; i < bd.biomesCount - 1; i++) { weights[i] = bd.allBiomesData[i + 1].weight; }
        var biomeIndex = GetBiomeIndexAt(tileIndex, bd.seed, bd.borderSize, bd.jitter, bd.biomesCount, weights, bd.biomeSize);
        if (!BiomesIndexes.TryGetValue(biomeIndex, out var type)) { preset = null; biomeSettings = null; return false; }
        preset = null;
        biomeSettings = null;
        foreach (var settings in InjectBiomesSettings.biomeSettings)
        {
            if (settings.biomeType != type) continue;
            if (settings.terrainGenerationPresets[0] is not IGenerationPreset generationPreset) continue;
            preset = generationPreset;
            biomeSettings = settings;
        }
        return preset != null;
    }

    private static int GetTileIndex(float noise, int maxIndex, BiomeSettingsSo biomeSettings)
    {
        foreach (var tile in biomeSettings.biomeTiles)
        {
            if (noise < tile.noise.x || noise > tile.noise.y) continue;
            var index = tile.tileAtlasIndex;
            return index > maxIndex ? maxIndex : index;
        }
        return 0;
    }
}
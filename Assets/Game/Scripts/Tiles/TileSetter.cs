using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameDataInjector;

public static class TileSetter
{
    public static Dictionary<TerrainType, Material> GetBiomeMaterials(TerrainDataGenerator[] generators)
    {
        var biomeMaterials = new Dictionary<TerrainType, Material>();
        foreach (var generator in generators) { biomeMaterials.Add(generator.terrainType, generator.terrainMaterial); }
        return biomeMaterials;
    }

    public static List<TerrainType> GetExcludedFromTileSmootherBiomes(TerrainDataGenerator[] generators)
    {
        var excludedBiomes = new List<TerrainType>();
        foreach (var generator in generators)
        {
            if (generator.excludeFromTileSmoother) excludedBiomes.Add(generator.terrainType);
        }
        return excludedBiomes;
    }
    
    public static Dictionary<Vector2Int, Tile> GetTilesInChunk(Vector2Int chunkIndex, TerrainDataGenerator[] biomeGenerators)
    {
        var chunkTiles = new Dictionary<Vector2Int, Tile>();
        var generators = biomeGenerators.OrderBy(gen => gen.generationOrder).ToList();
        foreach (var generator in generators)
        {
            var tiles = generator.GenerateTerrainTilesInChunk(chunkIndex);
            foreach (var tile in tiles)
            {
                if (tile == null) continue;
                chunkTiles.TryAdd(tile.Position, tile);
            }
        }
        return chunkTiles;
    }
    
    public static void FillEmptyTilesWithDefaultBiome(Dictionary<Vector2Int, Tile> chunkTiles, Vector2Int chunkIndex)
    {
        var chunkSize = InjectMapData.chunkSize;
        for (var y = 0; y < chunkSize; y++)
        {
            for (var x = 0; x < chunkSize; x++)
            {
                var xGlobal = chunkIndex.x * chunkSize + x;
                var yGlobal = chunkIndex.y * chunkSize + y;
                var position = new Vector2Int(xGlobal, yGlobal);
                if (chunkTiles.ContainsKey(position)) continue;
                var tile = new Tile();
                tile.Position = position;
                tile.TerrainType = InjectMapSettings.defaultTerrainType;
                chunkTiles.Add(position, tile);
            }
        }
    }
    
    public static void ClearUncommonTilesInChunk(Dictionary<Vector2Int, Tile> chunkTiles, List<TerrainType> excludedTypes = null)
    {
        foreach (var (tilePos, tile) in chunkTiles)
        {
            if (excludedTypes != null && excludedTypes.Contains(tile.TerrainType)) continue;
            var tileNeighborsBiome = new Dictionary<TerrainType, int>();
            for (var y = -1; y <= 1; y++)
            {
                for (var x = -1; x <= 1; x++)
                {
                    var nextTilePos = tilePos + new Vector2Int(x, y);
                    if (nextTilePos == tilePos) continue;
                    if (!chunkTiles.TryGetValue(nextTilePos, out var nextTile)) continue;
                    var nextTileBiome = nextTile.TerrainType;
                    tileNeighborsBiome.TryAdd(nextTileBiome, 0);
                    tileNeighborsBiome[nextTileBiome]++;
                }
            }
            var dominatingBiome = tile.TerrainType;
            var dominatingBiomeCount = 4;
            foreach (var biome in tileNeighborsBiome)
            {
                if (biome.Value <= dominatingBiomeCount) continue;
                dominatingBiome = biome.Key;
                dominatingBiomeCount = biome.Value;
            }
            tile.TerrainType = dominatingBiome;
        }
    }
}
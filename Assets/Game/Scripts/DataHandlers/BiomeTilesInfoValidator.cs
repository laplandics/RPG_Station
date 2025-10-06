using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiomeTilesInfoValidator
{
    private TileSettingsSo[] _tiles;
    private BiomeSettingsSo _biome;
    private bool _isErrorFound;
    private Dictionary<float, TileSettingsSo> _tilesNoiseMin = new();
    private Dictionary<float, TileSettingsSo> _tilesNoiseMax = new();
    
    public void ValidateTiles(TileSettingsSo[] tiles, BiomeSettingsSo biome)
    {
        _tiles = tiles;
        _biome = biome;
        if(IsArrayNullOrEmpty()) _isErrorFound = true;
        for (var i = 0; i < tiles.Length; i++)
        {
            var tile = tiles[i];
            if (IsTileEmpty(tile, i)) _isErrorFound = true;
        }
        if (IsIndexIncorrect()) _isErrorFound = true;
        if (IsKeyIncorrect()) _isErrorFound = true;
        if (IsNoiseCrossedOrGap()) _isErrorFound = true;
        
        if(!_isErrorFound)Debug.Log("Everything's fine");
    }

    private bool IsArrayNullOrEmpty()
    {
        if (_tiles != null && _tiles.Length != 0) return false;
        Debug.LogWarning($"{_biome.name} does not contain any tiles");
        return true;
    }

    private bool IsTileEmpty(TileSettingsSo tile, int index)
    {
        if (tile != null) return false;
        Debug.LogWarning($"{_biome.name}: on {index} tile is null");
        return true;
    }

    private bool IsIndexIncorrect()
    {
        var groups = _tiles.GroupBy(t => t.tileAtlasIndex).Where(g => g.Count() > 1).ToList();
        if (groups.Count == 0) return false;
        foreach (var group in groups)
        {
            Debug.LogWarning($"Index {group.Key} is the same between {string.Join(", ", group.Select(t => t.name))}");
        }
        return true;
    }

    private bool IsKeyIncorrect()
    {
        var groups = _tiles.GroupBy(t => t.tileKey).Where(g => g.Count() > 1).ToList();
        if (groups.Count == 0) return false;
        foreach (var group in groups)
        {
            Debug.LogWarning($"Key {group.Key} is the same between {string.Join(", ", group.Select(t => t.name))}");
        }
        return true;
    }

    private bool IsNoiseCrossedOrGap()
    {
        var sortedTiles = _tiles
            .Where(t => t != null)
            .Select(t => new {tile = t, min = t.noise.x, max = t.noise.y})
            .OrderBy(t => t.min)
            .ToList();
        for (var i = 0; i < sortedTiles.Count - 1; i++)
        {
            var current = sortedTiles[i];
            var next = sortedTiles[i + 1];
            var diff = Round2(next.min - current.max);
            switch (diff)
            {
                case > 0f:
                    Debug.LogWarning($"Gap between {current.tile.name} and {next.tile.name}");
                    return true;
                case < 0f:
                    Debug.LogWarning($"Noise is crossed between {current.tile.name} and {next.tile.name}");
                    return true;
            }
        }
        return false;
        float Round2(float v) => Mathf.Round(v * 100f) / 100f;
    }
}
using System;using System.Collections.Generic;
using UnityEngine;
using static EventManager;
using static GlobalMapMethods;
using static PerlinNoiseMap;
public class TerrainDataGenerator : IDisposable
{
    private readonly int _chunkSize;
    private readonly int _mapSize;
    private readonly int _seed;
    private readonly float _scale;
    private readonly float _multX;
    private readonly float _multY;
    private readonly int _threshold;

    public Dictionary<Vector2Int, TerrainType> Terrain { get; private set; } = new();

    public TerrainDataGenerator(MapData mapData)
    {
        _chunkSize = mapData.chunkSize;
        _mapSize = mapData.mapSize;
        _seed = mapData.seed;
        _scale = mapData.scale;
        _multX = mapData.multX;
        _multY = mapData.multY;
        _threshold = mapData.threshold;

        OnRenderAreaBorderReached.AddListener(GenerateTerrain);
    }

    private void GenerateTerrain(Vector2Int center)
    {
        Terrain.Clear();
        Debug.Log("Border Reached");
        var positions = GetBoundaries(_mapSize, center);
        var terrain = new Dictionary<Vector2Int, TerrainType>();
        foreach (var position in positions)
        {
            var noisePosX = position.x / _chunkSize;
            var noisePosY = position.y / _chunkSize;
            var noise = GetPerlinNoise(noisePosX, noisePosY, _scale, _seed, _multX, _multY);
            var chunkTerrain = ChooseChunkFromNoise(noise);
            terrain.TryAdd(position, chunkTerrain);
        }
        var smoothTerrain = new Dictionary<Vector2Int, TerrainType>();

        for (var i = 0; i < 3; i++) { smoothTerrain = SmoothTerrain(terrain, _threshold, _chunkSize); }
        foreach (var kvp in smoothTerrain) { Terrain.TryAdd(kvp.Key, kvp.Value); }
    }
    
    public void Dispose()
    {
        Terrain.Clear();
        OnRenderAreaBorderReached.RemoveListener(GenerateTerrain);
    }
}
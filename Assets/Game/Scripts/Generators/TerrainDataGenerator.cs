using UnityEngine;
using static PerlinNoiseMap;
using System.Collections.Generic;
using static EventManager;

public class TerrainDataGenerator
{
    private float _scale;
    private int _seed;
    private float _multX;
    private float _multY;
    private int _threshold;
    private MapManagerSo _mapManager;
    private MapData _mapData;
    private MapDataStorage _mapDataStorage;
    public void Initialize(MapManagerSo mapManager, MapData mapData, MapDataStorage mapDataStorage)
    {
        onRenderAreaBorderReached.RemoveListener(GenerateTerrain);

        _mapManager = mapManager;
        _mapData = mapData;
        _mapDataStorage = mapDataStorage;

        _scale = _mapData.scale;
        _seed = _mapData.seed;
        _multX = _mapData.multX;
        _multY = _mapData.multY;
        _threshold = _mapData.threshold;

        onRenderAreaBorderReached.AddListener(GenerateTerrain);
    }

    private void GenerateTerrain(Vector2 center)
    {
        _mapManager.terrain.Clear();
        Debug.Log("Border Reached");
        var centerInt = new Vector2Int((int)center.x, (int)center.y);
        var positions = _mapManager.GetBoundaries(_mapData.mapSize, centerInt);
        var terrain = new Dictionary<Vector2Int, TerrainType>();
        foreach (var position in positions)
        {
            var noisePosX = position.x / _mapData.chunkSize;
            var noisePosY = position.y / _mapData.chunkSize;
            var noise = GetPerlinNoise(noisePosX, noisePosY, _scale, _seed, _multX, _multY);
            var chunkTerrain = ChooseChunk(noise);
            terrain.TryAdd(position, chunkTerrain);
        }
        var smoothTerrain = new Dictionary<Vector2Int, TerrainType>();

        for (var i = 0; i < 3; i++) { smoothTerrain = SmoothTerrain(terrain, _threshold, _mapData.chunkSize); }
        foreach (var newTerrain in smoothTerrain) { _mapManager.terrain.TryAdd(newTerrain.Key, newTerrain.Value); }
    }

    private TerrainType ChooseChunk(float noise)
    {
        var type = TerrainType.Water;
        foreach (var chunkPrefab in _mapDataStorage.terrainChunks)
        {
            if (chunkPrefab is not ITerrainChunk terrain) continue;
            if (noise > terrain.NoiseMax || noise < terrain.NoiseMin) continue;
            type = terrain.TerrainType;
        }
        return type;
    }
}
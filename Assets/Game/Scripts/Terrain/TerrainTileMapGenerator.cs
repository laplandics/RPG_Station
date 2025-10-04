using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static EventManager;
using static GlobalMapMethods;
using static TilesDataGenerator;

public class TerrainTileMapGenerator : IDisposable
{
    private readonly Dictionary<Vector2Int, Chunk> _chunks = new();
    private readonly Queue<Vector2Int> _memorizedChunks;
    private readonly TileSetSo _tileSet;
    private readonly Tilemap _tilemap;
    private readonly RoutineService _routineService;
    private readonly int _renderChunksCount;
    private readonly int _chunkSize;
    private readonly int _memorizedArea;
    private bool _isFirstGeneration;

    public TerrainTileMapGenerator(Map map)
    {
        _isFirstGeneration = true;
        
        var mapData = MapDataHandler.GetMapData;
        var mapSettings = MapDataHandler.GetMapSettingsSo;
        _routineService = DS.GetSceneManager<RoutineService>();
        
        _renderChunksCount = mapData.renderChunksCount;
        _chunkSize = mapData.chunkSize;
        _memorizedArea = mapData.memorizedArea;
        _memorizedChunks = mapSettings.MemorizedChunks;
        
        _tileSet = mapSettings.tileSet;
        _tilemap = map.tileMap;
        
        OnSmbEnteredChunk.AddListener(StartBuildingTerrain);
        RestoreMemorizedArea();
    }

    private void RestoreMemorizedArea()
    {
        if (_memorizedChunks.Count <= 0) return;
        foreach (var position in _memorizedChunks) { GenerateChunk(position); }
    }

    private void StartBuildingTerrain(Vector2Int chunkIndex, IWalkable entity)
    {
        _routineService.StartRoutine(BuildNewTerrain(chunkIndex, entity));
    }
    
    private IEnumerator BuildNewTerrain(Vector2Int chunkIndex, IWalkable entity)
    {
        if (entity is not PlayerController) yield break;
        var indexes = GetChunksIndexesInArea(_renderChunksCount, chunkIndex);
        var nearChunks = new List<Vector2Int>();
        foreach (var index in indexes)
        {
            GenerateChunk(index, nearChunks);
            UpdateCalculationArea(index);
            yield return null;
        }
        ClearFarChunks(nearChunks);
        if (_isFirstGeneration) OnSceneReady?.Invoke();
        _isFirstGeneration = false;
    }
    
    private void GenerateChunk(Vector2Int index, List<Vector2Int> nearChunks = null)
    {
        nearChunks?.Add(index);
        if (_chunks.ContainsKey(index)) return;
        CreateChunk(index);
    }
    
    private void UpdateCalculationArea(Vector2Int index)
    {
        if (_memorizedChunks.Contains(index)) return;
        _memorizedChunks.Enqueue(index);
        if (_memorizedChunks.Count > _memorizedArea) _memorizedChunks.Dequeue();
    }

    private void ClearFarChunks(List<Vector2Int> nearChunks)
    {
        var farChunks = new List<Vector2Int>();
        foreach (var chunk in _chunks)
        {
            if (_memorizedChunks.Contains(chunk.Key)) continue;
            if (nearChunks.Contains(chunk.Key)) continue;
            farChunks.Add(chunk.Key);
            foreach (var tilePos in chunk.Value.TileBiomePairs.Keys)
            {
                _tilemap.SetTile((Vector3Int)tilePos, null);
            }
        }
        foreach (var farChunk in farChunks) { _chunks.Remove(farChunk); }
    }

    private void CreateChunk(Vector2Int chunkIndex)
    {
        var tiles = new Dictionary<Vector2Int, int>();
        
        var startTileX = chunkIndex.x * _chunkSize;
        var startTileY = chunkIndex.y * _chunkSize;
        var bounds = new BoundsInt(startTileX, startTileY, 0, _chunkSize, _chunkSize, 1);
        var tilesInBounds = new TileBase[_chunkSize * _chunkSize];
        var i = 0;
        for (var y = 0; y < _chunkSize; y++)
        {
            for (var x = 0; x < _chunkSize; x++)
            {
                var globalTileX = startTileX + x;
                var globalTileY = startTileY + y;

                var tileIndex = GetTileIndexByNoise(globalTileX, globalTileY);
                var tile = _tileSet.tiles[tileIndex];
                
                tilesInBounds[i] = tile;
                tiles.TryAdd(new Vector2Int(globalTileX, globalTileY), tileIndex);
                i++;
            }
        }
        _tilemap.SetTilesBlock(bounds, tilesInBounds);
        var chunk = new Chunk();
        chunk.Initialize(chunkIndex, tiles);
        _chunks.TryAdd(chunkIndex, chunk);
    }

    public void Dispose()
    {
        OnSmbEnteredChunk.RemoveListener(StartBuildingTerrain);
        foreach (var kvp in _chunks)
        {
            foreach (var tilePos in kvp.Value.TileBiomePairs.Keys) { _tilemap.SetTile((Vector3Int)tilePos, null); }
        }
        _chunks.Clear();
    }
}
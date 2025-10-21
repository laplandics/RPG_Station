using System;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;
using static MapHelper;
using static MemorizedAreaHandler;
using static GameDataInjector;

public class TerrainGenerator : IDisposable
{
    private readonly Dictionary<Vector2Int, Chunk> _chunks = new();
    private readonly ChunkCreator _chunkCreator;
    private readonly MapMajorSettingsSo _mapSettings;
    private readonly GOService _goService;
    private readonly Transform _mapTransform;
    private readonly int _renderChunksCount;
    private bool _isFirstGeneration;

    public TerrainGenerator(Map map)
    {
        _goService = DS.GetSceneManager<GOService>();
        _chunkCreator = new ChunkCreator();
        var mapData = InjectMapData;
        _mapTransform = map.transform;
        _mapSettings = InjectMapSettings;
        _renderChunksCount = mapData.renderChunksCount;
        _isFirstGeneration = true;
        OnSmbEnteredChunk.AddListener(BuildNewTerrain);
        RestoreMemorizedArea(_mapSettings.MemorizedChunks, index => GenerateChunk(index), mapData.memorizedArea);
    }
    
    private void BuildNewTerrain(Vector2Int chunkIndex, IWalkable entity)
    {
        if (entity is not Player) return;
        var indexes = GetChunksIndexesInArea(_renderChunksCount, chunkIndex);
        var nearChunks = new List<Vector2Int>();
        foreach (var index in indexes)
        {
            GenerateChunk(index, nearChunks);
            UpdateMemorizedArea(_mapSettings.MemorizedChunks, index);
        }
        ClearFarChunks(nearChunks);
        if (_isFirstGeneration) OnSceneReady?.Invoke();
        _isFirstGeneration = false;
    }
    
    private void GenerateChunk(Vector2Int index, List<Vector2Int> nearChunks = null)
    {
        nearChunks?.Add(index);
        if (_chunks.ContainsKey(index)) return;
        var chunk = _chunkCreator.CreateChunk(index, _mapTransform);
        _chunks.Add(index, chunk);
    }

    private void ClearFarChunks(List<Vector2Int> nearChunks)
    {
        var farChunks = new List<Vector2Int>();
        foreach (var chunk in _chunks)
        {
            if (_mapSettings.MemorizedChunks.Contains(chunk.Key)) continue;
            if (nearChunks.Contains(chunk.Key)) continue;
            farChunks.Add(chunk.Key);
            _goService.Despawn(chunk.Value.gameObject);
        }
        foreach (var farChunk in farChunks) { _chunks.Remove(farChunk); }
    }
    
    public Dictionary<Vector2Int, Chunk> GetChunks() => _chunks;

    public void Dispose()
    {
        OnSmbEnteredChunk.RemoveListener(BuildNewTerrain);
        foreach (var kvp in _chunks) { if (kvp.Value != null) _goService.Despawn(kvp.Value.gameObject); }
        _chunks.Clear();
    }
}
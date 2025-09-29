using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;
using static GlobalMapMethods;

public class TerrainChunksGenerator : IDisposable
{
    private readonly RoutineService _routineService;
    private readonly GOService _goService;
    private readonly MapInitializer _initializer;
    private readonly Transform _mapT;
    private Coroutine _generateCoroutine;
    private readonly int _renderAreaSize;
    private bool _isGenerating;
    private bool _isFirstGeneration;
    
    private Dictionary<Vector2Int, Chunk> Chunks { get; set; } = new();

    public TerrainChunksGenerator(MapInitializer initializer, Map map, MapData mapData)
    {
        _goService = DS.GetSceneManager<GOService>();
        _initializer = initializer;
        _mapT = map.transform;
        _renderAreaSize = mapData.renderAreaSize;
        _isFirstGeneration = true;
        _routineService = DS.GetSceneManager<RoutineService>();
        OnPlayerChunkChanged.AddListener(GeneratingChunkRoutine);
    }

    private void GeneratingChunkRoutine(Vector2Int chunkPos)
    {
        _generateCoroutine = _routineService.StartRoutine(GenerateChunks(chunkPos));
    }

    private IEnumerator GenerateChunks(Vector2Int playerChunk)
    {
        if (_isGenerating) yield break;
        _isGenerating = true;
        var nearChunks = new HashSet<Vector2Int>();
        var chunksPositions = GetBoundaries(_renderAreaSize, playerChunk);
        CheckTerrain(chunksPositions, playerChunk);
        foreach (var position in chunksPositions)
        {
            nearChunks.Add(position);
            if (Chunks.ContainsKey(position)) continue;
            if (!SpawnChunk(position)) yield break;
            yield return null;
        }
        var chunksToRemove = new List<Vector2Int>();
        foreach (var kvp in Chunks)
        {
            if (nearChunks.Contains(kvp.Key)) continue;
            _goService.Despawn(kvp.Value.gameObject);
            chunksToRemove.Add(kvp.Key);
        }
        foreach (var key in chunksToRemove) { Chunks.Remove(key); }

        _isGenerating = false;
        if (!_isFirstGeneration) yield break;
        _isFirstGeneration = false;
        OnSceneReady?.Invoke();
    }

    private void CheckTerrain(List<Vector2Int> chunksPositions, Vector2Int playerChunk)
    {
        foreach (var position in chunksPositions)
        {
            if (_initializer.TerrainDataGenerator.Terrain.ContainsKey(position)) continue;
            OnRenderAreaBorderReached?.Invoke(playerChunk);
        }
    }
    
    private Chunk SpawnChunk(Vector2Int position)
    {
        var chunkInstance = GetTerrain(position);
        if (!chunkInstance) return null;
        var pos = new Vector2(position.x, position.y);
        var chunk = _goService.Spawn(chunkInstance, pos, Quaternion.identity, _mapT);
        chunk.gameObject.name = $"Chunk {position.x}:{position.y}";
        Chunks.Add(position, chunk);
        return chunk;
    }
    
    private Chunk GetTerrain(Vector2Int position)
    {
        _initializer.TerrainDataGenerator.Terrain.TryGetValue(position, out var terrain);
        foreach (var terrainSettings in TerrainDataHandler.GetTerrainSettingsSo)
        {
            if (terrainSettings.terrainType != terrain) continue;
            return terrainSettings.chunkPrefab;
        }
        return null;
    }

    public void Dispose()
    {
        foreach (var chunk in Chunks.Values)
        {
            _goService.Despawn(chunk.gameObject);
        }
        Chunks.Clear();
        OnPlayerChunkChanged.RemoveListener(GeneratingChunkRoutine);
        if(_generateCoroutine != null) _routineService.EndRoutine(_generateCoroutine);
    }
}
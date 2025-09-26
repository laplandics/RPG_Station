using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static EventManager;

[CreateAssetMenu(fileName = "ChunksSpawner", menuName = "SpawnersSO/ChunksSpawner")]
public class ChunksSpawnManagerSo : ScriptableObject
{
    private RoutineManager _routineManager;
    private Transform _map;
    private Vector2Int _playerChunk;
    private MapDataStorage _mapDataStorage;
    private MapManagerSo _mapManager;
    private MapData _mapData;
    private bool _isFirstGeneration;
    public void InitializeSpawner(MapManagerSo mapManager, MapData data, MapDataStorage dataStorage)
    {
        _routineManager = DS.GetSceneManager<RoutineManager>();

        _mapManager = mapManager;
        _mapDataStorage = dataStorage;
        _mapData = data;

        _isFirstGeneration = true;
    }

    public void GenerateChunksAroundPlayer(Transform playerTransform, Transform mapTransform)
    {
        if (_map == null) _map = mapTransform;
        var newPlayerChunk = _mapManager.GetPlayersChunk(playerTransform);
        if (!_isFirstGeneration) { if (_playerChunk == newPlayerChunk) return; }
        _playerChunk = newPlayerChunk;
        _routineManager.StartRoutine(GenerateChunksRoutine());
    }

    private IEnumerator GenerateChunksRoutine()
    {
        var nearChunks = new HashSet<Vector2Int>();
        var chunksPositions = _mapManager.GetBoundaries(_mapData.renderAreaSize, _playerChunk);
        CheckTerrain(chunksPositions, _playerChunk);
        foreach (var position in chunksPositions)
        {
            nearChunks.Add(position);
            if (_mapManager.sceneChunks.ContainsKey(position)) continue;
            if (SpawnChunk(position) == null) yield break;
            yield return null;
        }

        var chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in _mapManager.sceneChunks)
        {
            if (nearChunks.Contains(chunk.Key)) continue;
            Destroy(chunk.Value.gameObject);
            chunksToRemove.Add(chunk.Key);
        }
        foreach (var chunk in chunksToRemove)
        {
            _mapManager.sceneChunks.Remove(chunk);
        }

        if (!_isFirstGeneration) yield break;
        _isFirstGeneration = false;
        onSceneReady?.Invoke();
    }

    private Chunk SpawnChunk(Vector2Int position)
    {
        var chunkInstance = GetTerrain(position);
        if (chunkInstance == null) return null;
        var chunk = Instantiate(chunkInstance, new Vector3Int(position.x, position.y, 0), Quaternion.identity, _map);
        _mapManager.sceneChunks.TryAdd(position, chunk);
        chunk.gameObject.name = $"Chunk ({position.x}:{position.y})";

        return chunk;
    }

    private Chunk GetTerrain(Vector2Int position)
    {
        foreach (var terrain in _mapManager.terrain)
        {
            if (terrain.Key != position) continue;
            foreach (var chunk in _mapDataStorage.terrainChunks)
            {
                if (chunk is not ITerrainChunk terrainChunk) continue;
                if (terrainChunk.TerrainType != terrain.Value) continue;
                return chunk;
            }
        }
        return null;
    }

    private void CheckTerrain(List<Vector2Int> currentRenderPositions, Vector2Int playerPos)
    {
        foreach (var position in currentRenderPositions)
        {
            if (_mapManager.terrain.ContainsKey(position)) continue;
            onRenderAreaBorderReached?.Invoke(playerPos);
        }
    }
}
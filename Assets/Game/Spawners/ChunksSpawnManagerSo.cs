using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ChunksSpawner", menuName = "SpawnersSO/ChunksSpawner")]
public class ChunksSpawnManagerSo : ScriptableObject, ISpawner
{
    private Map _map;
    private Transform _playerTransform;
    private Transform _mapTransform;
    private EventManagerSo _eventManager;
    private PlayerManagerSo _playerManager;
    private ChunksManagerSo _chunkManager;
    private RoutineManager _routineManager;
    private HashSet<Vector2Int> nearChunks = new();
    public void InitializeSpawner()
    {
        _eventManager = DS.GetSoManager<EventManagerSo>();
        _playerManager = DS.GetSoManager<PlayerManagerSo>();
        _chunkManager = DS.GetSoManager<ChunksManagerSo>();
        _routineManager = DS.GetSceneManager<RoutineManager>();

        _eventManager.onMapGenerated.AddListener(GenerateChunksAroundPlayer);

    }

    public void GenerateChunksAroundPlayer(Map map = null)
    {
        if (_map == null) _map = map;
        if (_mapTransform == null && map != null) _mapTransform = map.transform;

        SpawnChunksRoutine();
    }

    private void SpawnChunksRoutine()
    {
        var chunksPositions = GetBoundaries();
        foreach (var position in chunksPositions)
        {
            nearChunks.Add(position);
            if (_chunkManager.ChunksInScene.ContainsKey(position)) continue;
            SpawnChunk(position);
        }

        var chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in _chunkManager.ChunksInScene)
        {
            if (nearChunks.Contains(chunk.Key)) continue;
            chunk.Value.EraseChunk(true);
            Destroy(chunk.Value.gameObject);
            chunksToRemove.Add(chunk.Key);
        }
        foreach (var chunk in chunksToRemove)
        {
            _chunkManager.ChunksInScene.Remove(chunk);
        }
        chunksToRemove.Clear();
        nearChunks.Clear();
    }

    private List<Vector2Int> GetBoundaries()
    {
        var boundaries = new List<Vector2Int>();
        var center = GetPlayerChunk();
        for (var y = -_map.RenderAreaSize; y <= _map.RenderAreaSize; y++)
        {
            for (var x = -_map.RenderAreaSize; x <= _map.RenderAreaSize; x++)
            {
                var chunkX = center.x + x;
                var chunkY = center.y + y;
                boundaries.Add(new Vector2Int(chunkX * _map.ChunkSize, chunkY * _map.ChunkSize));
            }
        }
        return boundaries;
    }

    public Vector2Int GetPlayerChunk()
    {
        _playerTransform = _playerManager.GetPlayerTransform();
        var player = _playerTransform.position;
        var playerChunkX = (int)Math.Round(player.x / _map.ChunkSize);
        var playerChunkY = (int)Math.Round(player.y / _map.ChunkSize);

        return new Vector2Int(playerChunkX, playerChunkY);
    }

    private Chunk SpawnChunk(Vector2Int position)
    {
        var chunkInstance = TryGetSavedChunk(position, out var chunkData);
        var chunk = Instantiate(chunkInstance, new Vector3Int(position.x, position.y, 0), Quaternion.identity, _mapTransform);
        chunk.gameObject.name = $"Chunk ({position.x}:{position.y})";
        chunk.ChunkData = chunkData;
        chunk.SetChunk();
        chunk.Load();
        _chunkManager.ChunksInScene.TryAdd(position, chunk);

        return chunk;
    }

    private Chunk TryGetSavedChunk(Vector2Int pos, out ChunkData data)
    {
        var savedChunks = _chunkManager.SavedChunks;
        if (savedChunks.Count != 0)
        {
            foreach (var savedChunk in savedChunks)
            {
                if (savedChunk.Key != pos) continue;
                foreach (var chunkPrefab in _map.TerrainChunks)
                {
                    if (chunkPrefab.InstanceKey != savedChunk.Value.prefabKeyData) continue;
                    data = savedChunk.Value;
                    return chunkPrefab;
                }
            }
        }
        foreach (var chunkFile in _map.MapData.savedChunksData)
        {
            if (chunkFile.positionData != pos) continue;
            foreach (var chunkPrefab in _map.TerrainChunks)
            {
                if (chunkPrefab.InstanceKey != chunkFile.prefabKeyData) continue;
                data = chunkFile;
                return chunkPrefab;
            }
        }

        var chunkRandomData = _map.TerrainChunks[Random.Range(0, _map.TerrainChunks.Length)];
        data = new ChunkData();
        return chunkRandomData;
    }

    public void ClearAllChunks()
    {
        foreach (var chunk in _chunkManager.ChunksInScene)
        {
            chunk.Value.EraseChunk(false);
            Destroy(chunk.Value.gameObject);
        }
        _chunkManager.ChunksInScene.Clear();
    }

}
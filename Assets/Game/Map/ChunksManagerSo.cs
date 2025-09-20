using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ChunksManager", menuName = "ManagersSO/ChunksManager")]
public class ChunksManagerSo : ScriptableObject
{
    [SerializeField] private int chunkSize;
    private Dictionary<Vector3, Chunk> _chunks = new();
    private EventManagerSo _eventManager;
    private MapManagerSo _mapManager;
    private Transform _mapTransform;
    private PlayerManagerSo _playerManager;
    private Transform _playerTransform;
    private Vector3 _playerChunkPosition;
    private bool _firstGeneration;

    public Task Initialize()
    {
        _firstGeneration = true;
        _eventManager = DS.GetSoManager<EventManagerSo>();
        _eventManager.onMapUpdated.AddListener(() => _ = GenerateChunks());
        _eventManager.onPlayersPositionChanged.AddListener(() => _ = GenerateChunks());

        _mapManager = DS.GetSoManager<MapManagerSo>();
        _playerManager = DS.GetSoManager<PlayerManagerSo>();

        return Task.CompletedTask;
    }

    private async Task GenerateChunks()
    {
        var center = GetPlayerChunk();
        var nearChunks = new HashSet<Vector3>();
        if (!_firstGeneration)
        {
            if (_playerChunkPosition == center) return;
            _playerChunkPosition = center;
        }

        var chunksPositions = GetBoundaries();
        foreach (var position in chunksPositions)
        {
            nearChunks.Add(position);
            if (_chunks.ContainsKey(position)) continue;
            var chunk = await SpawnChunk(position);
            _eventManager.onChunkSpawned?.Invoke(chunk);
        }
        _firstGeneration = false;
        var chunksToRemove = new List<Vector3>();
        foreach (var chunk in _chunks)
        {
            if (nearChunks.Contains(chunk.Key)) continue;
            await DespawnChunk(chunk.Value.gameObject);
            chunksToRemove.Add(chunk.Key);
        }
        foreach (var chunk in chunksToRemove)
        {
            _chunks.Remove(chunk);
        }
        chunksToRemove.Clear();
    }

    public async Task LoadChunks()
    {
        foreach (var chunk in _chunks.Values)
        {
            await DespawnChunk(chunk.gameObject);
        }
        _chunks.Clear();

        var savedChunks = _mapManager.Map.GetSavedChunks();
        foreach (var chunkData in savedChunks)
        {
            var chunk = await SpawnChunk(chunkData.position);
            if (chunk == null) continue;
            await LoadChunkData(chunk, chunkData);
            _eventManager.onChunkSpawned?.Invoke(chunk);
        }
        
    }

    public async Task SaveChunksData()
    {
        foreach (var chunk in _chunks.Values)
        {
            await chunk.Save();
        }
    }

    private async Task<Chunk> SpawnChunk(Vector3 position)
    {
        if (!GetBoundaries().Contains(position)) return null;
        _mapTransform = _mapManager.Map.transform;
        var chunkInstance = TryGetSavedChunk(position, out var savedData);
        var chunk = Instantiate(chunkInstance, position, Quaternion.identity, _mapTransform);
        if (savedData != null) await LoadChunkData(chunk, savedData);
        chunk.gameObject.name = $"Chunk ({position.x}:{position.y})";
        _chunks.TryAdd(position, chunk);

        return chunk;
    }

    private Task DespawnChunk(GameObject chunk)
    {
        Destroy(chunk);
        _eventManager.onChunkDespawned?.Invoke(chunk.GetComponent<Chunk>());
        return Task.CompletedTask;
    }

    private Vector3 GetPlayerChunk()
    {
        _playerTransform = _playerManager.GetPlayerTransform();
        var player = _playerTransform.position;
        var playerChunkX = (int)Math.Round(player.x / chunkSize);
        var playerChunkY = (int)Math.Round(player.y / chunkSize);

        return new Vector3(playerChunkX, playerChunkY, 0f);
    }

    private List<Vector3> GetBoundaries()
    {
        var boundaries = new List<Vector3>();
        var center = GetPlayerChunk();
        for (var y = -20; y <= 20; y++)
        {
            for (var x = -20; x <= 20; x++)
            {
                var chunkX = center.x + x;
                var chunkY = center.y + y;
                boundaries.Add(new Vector3(chunkX * chunkSize, chunkY * chunkSize, 0f));
            }
        }
        return boundaries;
    }

    private Chunk TryGetSavedChunk(Vector3 pos, out ChunkData chunkData)
    {
        var savedChunks = _mapManager.Map.GetSavedChunks();
        if (savedChunks != null)
        {
            foreach (var savedChunk in savedChunks)
            {
                if (savedChunk.position != pos) continue;
                foreach (var chunk in _mapManager.MapChunks)
                {
                    if (chunk.PrefabKey == savedChunk.prefabKey)
                    {
                        chunkData = savedChunk;
                        return chunk;
                    }
                }
            }
        }

        var prefab = _mapManager.MapChunks[Random.Range(0, _mapManager.MapChunks.Count)];
        chunkData = null;
        return prefab;
    }

    private async Task LoadChunkData(Chunk chunk, ChunkData chunkData)
    {
        chunk.ChunkData = chunkData;
        await chunk.Load();
    }
}

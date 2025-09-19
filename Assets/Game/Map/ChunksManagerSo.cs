using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ChunksManager", menuName = "ManagersSO/ChunksManager")]
public class ChunksManagerSo : ScriptableObject
{
    [SerializeField] private MainGameObjectsSo instances;
    [SerializeField] private int chunkSize;
    private Dictionary<Vector3, Chunk> _chunks = new();
    private EventManagerSo eventManager;
    private Transform _map;
    private Vector3 _playersChunkPosition;
    private bool _firstTimeGeneration;

    public Task Initialize()
    {
        _firstTimeGeneration = true;
        eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onMapSpawned.AddListener(map => _ = GenerateChunks(map));
        eventManager.onPlayersPositionChanged.AddListener(() => _ = UpdateChunks());
        return Task.CompletedTask;
    }

    private async Task GenerateChunks(Map map)
    {
        _map = map.GetComponent<Transform>();
        await GenerateAroundPlayer();
    }
    
    private async Task UpdateChunks()
    {
        await GenerateAroundPlayer();
        eventManager.onMapUpdated?.Invoke();
    }
    
    private async Task GenerateAroundPlayer()
    {
        var center = GetPlayerChunk();
        var nearChunks = new HashSet<Vector3>();
        if (!_firstTimeGeneration)
        {
            if (_playersChunkPosition == center) return;
            _playersChunkPosition = center;
        }

        var chunksPositions = GetBoundaries();
        foreach (var position in chunksPositions)
        {
            nearChunks.Add(position);
            if (_chunks.ContainsKey(position)) continue;
            var chunkPrefab = TryGetSavedChunk(position);
            var chunk = Instantiate(chunkPrefab, position, chunkPrefab.transform.rotation, _map);
            chunk.gameObject.name = $"Chunk ({position.x}:{position.y})";
            _chunks.TryAdd(position, chunk.GetComponent<Chunk>());
            eventManager.onChunkSpawned?.Invoke(_chunks[position]);
            await Task.Yield();
        }
        _firstTimeGeneration = false;
        var chunksToRemove = new List<Vector3>();
        foreach (var chunk in _chunks)
        {
            if (nearChunks.Contains(chunk.Key)) continue;
            Destroy(chunk.Value.gameObject);
            eventManager.onChunkDespawned?.Invoke(chunk.Value);
            chunksToRemove.Add(chunk.Key);
        }
        foreach (var chunk in chunksToRemove)
        {
            _chunks.Remove(chunk);
        }
        chunksToRemove.Clear();
    }

    private List<Vector3> GetBoundaries()
    {
        var boundaries = new List<Vector3>();
        var center = GetPlayerChunk();
        for (var y = -1; y <= 1; y++)
        {
            for (var x = -1; x <= 1; x++)
            {
                var chunkX = center.x + x;
                var chunkY = center.y + y;
                boundaries.Add(new Vector3(chunkX * chunkSize, chunkY * chunkSize, 0f));
            }
        }
        return boundaries;
    }

    private Vector3 GetPlayerChunk()
    {
        var player = DS.GetSoManager<PlayerManagerSo>().GetPlayerPosition();
        var playerChunkX = (int)Math.Round(player.x / chunkSize);
        var playerChunkY = (int)Math.Round(player.y / chunkSize);

        return new Vector3(playerChunkX, playerChunkY, 0f);
    }

    private Chunk TryGetSavedChunk(Vector3 position)
    {
        var mapManager = DS.GetSoManager<MapManagerSo>();
        var savedChunks = mapManager.Map.GetSavedChunks();
        if (savedChunks != null)
        {
            foreach (var chunk in savedChunks)
            {
                if (chunk.position != position) continue;
                foreach (var prefab in mapManager.MapChunks)
                {
                    if (prefab.PrefabKey == chunk.prefabKey) return prefab;
                }
            }
        }

        return mapManager.MapChunks[Random.Range(0, mapManager.MapChunks.Count)];
    }

    public void DestroyAllChunks()
    {
        foreach (var chunk in _chunks.Values)
        {
            Destroy(chunk.gameObject);
            eventManager.onChunkDespawned?.Invoke(chunk);
        }
        _chunks.Clear();
    }

    public async Task SaveChunksData()
    {
        foreach (var chunk in _chunks.Values)
        {
            await chunk.Save();
        }
    }

    public async Task LoadChunksData(List<ChunkData> chunkData)
    {
        foreach (var savedChunk in chunkData)
        {
            var chunk = GenerateInKnownPosition(savedChunk.prefabKey, savedChunk.position);
            if (!chunk) continue;
            chunk.ChunkData = savedChunk;
            await chunk.Load();
        }
    }

    private Chunk GenerateInKnownPosition(string prefabKey, Vector3 position)
    {
        var mapManager = DS.GetSoManager<MapManagerSo>();
        var boundaries = GetBoundaries();
        if (!boundaries.Contains(position)) return null;
        foreach (var prefab in mapManager.MapChunks)
        {
            if (prefab.PrefabKey != prefabKey) continue;
            _chunks.TryAdd(position, Instantiate(prefab, position, Quaternion.identity, _map));
            eventManager.onChunkSpawned?.Invoke(_chunks[position]);
            return _chunks[position];
        }
        return null;
    }
}

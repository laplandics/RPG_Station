using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ChunksManager", menuName = "ManagersSO/ChunksManager")]
public class ChunksManagerSo : ScriptableObject
{
    [SerializeField] private InitInstances instances;
    [SerializeField] private int chunkSize;
    private Dictionary<Vector3, Chunk> _chunks = new();
    private Transform _map;
    private Vector3 _playersChunkPosition;
    private bool _firstTimeGeneration;
    
    public async Task SpawnChunks(Transform map)
    {
        Debug.LogWarning("TODO: Teleport player on mirrored coordinates and spawn same chunks on borders");
        _firstTimeGeneration = true;
        _map = map;
        DS.GetSoManager<EventManagerSo>().onPlayersPositionChanged.AddListener(() => _ = UpdateChunks());
        if (_chunks.Count == 0) await UpdateChunks();
    }
    
    private async Task UpdateChunks()
    {
        await GenerateAroundPlayer();
        DS.GetSoManager<EventManagerSo>().onMapUpdated?.Invoke();
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
            DS.GetSoManager<EventManagerSo>().onChunkSpawned?.Invoke(_chunks[position]);
            await Task.Yield();
        }
        _firstTimeGeneration = false;
        var chunksToRemove = new List<Vector3>();
        foreach (var chunk in _chunks)
        {
            if (nearChunks.Contains(chunk.Key)) continue;
            Destroy(chunk.Value.gameObject);
            DS.GetSoManager<EventManagerSo>().onChunkDespawned?.Invoke(chunk.Value);
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
        var savedChunks = DS.GetSoManager<MapManagerSo>().Map.GetSavedChunks();
        if (savedChunks != null)
        {
            foreach (var chunk in savedChunks)
            {
                if (chunk.position != position) continue;
                foreach (var prefab in instances.chunkPrefabs)
                {
                    if (prefab.PrefabKey == chunk.prefabKey) return prefab;
                }
            }
        }

        return instances.chunkPrefabs[Random.Range(0, instances.chunkPrefabs.Length)];
    }

    public void DestroyAllChunks()
    {
        foreach (var chunk in _chunks.Values)
        {
            Destroy(chunk.gameObject);
            DS.GetSoManager<EventManagerSo>().onChunkDespawned?.Invoke(chunk);
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
        var boundaries = GetBoundaries();
        if (!boundaries.Contains(position)) return null;
        foreach (var prefab in instances.chunkPrefabs)
        {
            if (prefab.PrefabKey != prefabKey) continue;
            _chunks.TryAdd(position, Instantiate(prefab, position, Quaternion.identity, _map));
            DS.GetSoManager<EventManagerSo>().onChunkSpawned?.Invoke(_chunks[position]);
            return _chunks[position];
        }
        return null;
    }
}

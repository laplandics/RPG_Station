using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Map : MonoBehaviour, ISaveAble
{
    [SerializeField] private string key;
    [SerializeField] private List<Chunk> currentChunks = new();
    private List<ChunkData> _savedChunks = new();
    private readonly CancellationTokenSource _cts = new();
    public string InstanceKey { get => key; set => key = value; }

    public void Initialize()
    {
        var eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onChunkSpawned.AddListener(chunk => _ = AddChunk(chunk));
        eventManager.onChunkDespawned.AddListener(RemoveChunk);
        eventManager.onMapUpdated?.Invoke();
    }

    public List<ChunkData> GetSavedChunks() => _savedChunks;

    private async Task AddChunk(Chunk chunk)
    {
        currentChunks.Add(chunk);
        await chunk.Save();
        foreach (var savedData in _savedChunks)
        {
            if (savedData.position == chunk.ChunkData.position) return;
        }
        _savedChunks.Add(chunk.ChunkData);
    }

    private void RemoveChunk(Chunk chunk) => currentChunks.Remove(chunk);
    
    public async Task Save()
    {
        var data = new MapData { savedChunks = _savedChunks };
        await DS.GetSoManager<SaveLoadManagerSo>().Save(key, data);
    }

    public async Task Load()
    {
        var data = await DS.GetSoManager<SaveLoadManagerSo>().Load<MapData>(key);
        _savedChunks = data.savedChunks;
        await DS.GetSoManager<ChunksManagerSo>().LoadChunks();
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
    }
}

[Serializable]
public class MapData
{
    public List<ChunkData> savedChunks;
}

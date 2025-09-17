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
    public string PrefabKey { get => key; set => key = value; }

    public void Initialize()
    {
        DS.GetSoManager<EventManagerSo>().onChunkSpawned.AddListener(AddChunk);
        DS.GetSoManager<EventManagerSo>().onChunkDespawned.AddListener(RemoveChunk);
    }

    public List<ChunkData> GetSavedChunks() => _savedChunks;

    private void AddChunk(Chunk chunk)
    {
        currentChunks.Add(chunk);
        chunk.Save();
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
        DS.GetSoManager<ChunksManagerSo>().DestroyAllChunks();
        await DS.GetSoManager<ChunksManagerSo>().LoadChunksData(data.savedChunks);
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

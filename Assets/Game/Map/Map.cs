using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour, ISaveAble
{
    [SerializeField] private string key;
    [SerializeField] private List<Chunk> currentChunks = new();
    private List<ChunkData> _savedChunksData = new();
    public string InstanceKey { get => key; set => key = value; }

    public void Initialize()
    {
        var eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onChunkSpawned.AddListener(chunk => AddChunk(chunk));
        eventManager.onChunkDespawned.AddListener(chunk => RemoveChunk(chunk));
        eventManager.onMapUpdated?.Invoke();
    }

    public List<ChunkData> GetSavedChunks() => _savedChunksData;

    private void AddChunk(Chunk chunk)
    {
        currentChunks.Add(chunk);
        chunk.Save();
        UpdateSavedChunksData(chunk);
    }

    private void RemoveChunk(Chunk chunk)
    {
        chunk.Save();
        UpdateSavedChunksData(chunk);
        currentChunks.Remove(chunk);
    }

    private void UpdateSavedChunksData(Chunk chunk)
    {
        for (var i = _savedChunksData.Count - 1; i >= 0; i--)
        {
            if (_savedChunksData[i].position == chunk.ChunkData.position) _savedChunksData.RemoveAt(i);
        }
        _savedChunksData.Add(chunk.ChunkData);
    }
    
    public void Save()
    {
        foreach (var chunk in currentChunks)
        {
            if (chunk is not ISaveAble saveAbleChunk) continue;
            saveAbleChunk.Save();
            UpdateSavedChunksData(chunk);
        }
        var data = new MapData { savedChunks = _savedChunksData };
        DS.GetSoManager<SaveLoadManagerSo>().Save(key, data);
    }

    public void Load()
    {
        var data = DS.GetSoManager<SaveLoadManagerSo>().Load<MapData>(key);
        _savedChunksData = data.savedChunks;
        DS.GetSoManager<ChunksManagerSo>().LoadChunks();
    }
}

[Serializable]
public class MapData
{
    public List<ChunkData> savedChunks;
}

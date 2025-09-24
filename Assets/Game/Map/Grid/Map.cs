using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour, ISaveAble
{
    [Header("Saveload id")]
    [SerializeField] private string prefabKey;
    [Header("Map settings")]
    [SerializeField] private Chunk[] terrainChunks;
    [SerializeField] private int chunkSize;
    [SerializeField] private int renderAreaSize;
    [SerializeField] private int prerenderMapSize;

    private ChunksManagerSo _chunksManager;

    public string InstanceKey { get => prefabKey; set => prefabKey = value; }
    public Chunk[] TerrainChunks => terrainChunks;
    public int ChunkSize => chunkSize;
    public int RenderAreaSize => renderAreaSize;
    public int PrerenderMapSize => prerenderMapSize;
    public MapData MapData { get; set; }

    public void Initialize()
    {
        _chunksManager = DS.GetSoManager<ChunksManagerSo>();
    }

    public void Load()
    {
        var data = DS.GetSoManager<SaveLoadManagerSo>().Load<MapData>(prefabKey);
        _chunksManager.Load(data.savedChunksData);
    }

    public void Save()
    {
        var savedChunks = _chunksManager.Save();
        foreach (var savedChunk in savedChunks)
        {
            foreach (var chunkFile in MapData.savedChunksData)
            {
                if (savedChunk.positionData != chunkFile.positionData) continue;
                chunkFile.enemiesData = savedChunk.enemiesData;
            }
        }
        
        DS.GetSoManager<SaveLoadManagerSo>().Save(prefabKey, MapData);
    }
}

[Serializable]
public class MapData
{
    public List<ChunkData> savedChunksData;
}

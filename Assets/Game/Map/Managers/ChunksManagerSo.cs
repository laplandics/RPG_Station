using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunksManager", menuName = "ManagersSO/ChunksManager")]
public class ChunksManagerSo : ScriptableObject
{
    private Dictionary<Vector2Int, Chunk> _chunksInScene = new();
    private Dictionary<Vector2Int, ChunkData> _savedChunks = new();
    private Vector2Int _currentPlayersChunk;
    private ChunksSpawnManagerSo _chunksSpawner;
    private EventManagerSo _eventManager;

    public Dictionary<Vector2Int, ChunkData> SavedChunks => _savedChunks;
    public Dictionary<Vector2Int, Chunk> ChunksInScene { get => _chunksInScene; set => _chunksInScene = value; }
    public void Initialize()
    {
        _chunksSpawner = DS.GetSoSpawner<ChunksSpawnManagerSo>();
        _eventManager = DS.GetSoManager<EventManagerSo>();

        _currentPlayersChunk = Vector2Int.zero;
        _eventManager.onChunkDespawned.AddListener(RemoveChunk);
        _eventManager.onPlayersPositionChanged.AddListener(TryChangePlayersChunk);
    }

    private void RemoveChunk(Vector2Int position, Chunk chunk, bool shouldSave)
    {
        if (shouldSave)
        {
            chunk.Save();
            if (!_savedChunks.TryAdd(position, chunk.ChunkData)) _savedChunks[position] = chunk.ChunkData;
        }
    }

    private void TryChangePlayersChunk(Transform _)
    {
        var playersChunk = _chunksSpawner.GetPlayerChunk();
        if (playersChunk == _currentPlayersChunk) return;
        _currentPlayersChunk = playersChunk;
        _chunksSpawner.GenerateChunksAroundPlayer();
    }

    public List<ChunkData> Save()
    {
        foreach (var chunk in _chunksInScene)
        {
            chunk.Value.Save();
            _savedChunks.TryAdd(chunk.Key, chunk.Value.ChunkData);
        }

        return _savedChunks.Values.ToList();
    }

    public void Load(List<ChunkData> chunkData)
    {
        _savedChunks.Clear();
        foreach (var data in chunkData)
        {
            if (!data.isVisitedData) continue;
            _savedChunks.TryAdd(data.positionData, data);
        }
        _chunksSpawner.ClearAllChunks();
        _chunksSpawner.GenerateChunksAroundPlayer();
    }
}
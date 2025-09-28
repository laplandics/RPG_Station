using UnityEngine;
using static EventManager;

public class ChunksGenerator
{
    private Transform _mapT;
    private int _chunkSize;
    private int _mapSize;
    private int _renderAreaSize;
    private int _seed;
    private float _scale;
    private float _multX;
    private float _multY;
    private int _threshold;

    public ChunksGenerator(Map map, MapData mapData)
    {
        _mapT = map.transform;
        _chunkSize = mapData.chunkSize;
        _mapSize = mapData.mapSize;
        _renderAreaSize = mapData.renderAreaSize;
        _seed = mapData.seed;
        _scale = mapData.scale;
        _multX = mapData.multX;
        _multY = mapData.multY;
        _threshold = mapData.threshold;
        
        OnPlayerChunkChanged.AddUniqueListener(GenerateChunks);
    }

    private void GenerateChunks(Vector2Int playerChunk)
    {
        Debug.Log("Generating chunks");
    }
}
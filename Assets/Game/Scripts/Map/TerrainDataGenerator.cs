using UnityEngine;
using static EventManager;
public class TerrainDataGenerator
{
    private int _chunkSize;
    private int _mapSize;
    private int _renderAreaSize;
    private int _seed;
    private float _scale;
    private float _multX;
    private float _multY;
    private int _threshold;

    public TerrainDataGenerator(MapData mapData)
    {
        _chunkSize = mapData.chunkSize;
        _mapSize = mapData.mapSize;
        _renderAreaSize = mapData.renderAreaSize;
        _seed = mapData.seed;
        _scale = mapData.scale;
        _multX = mapData.multX;
        _multY = mapData.multY;
        _threshold = mapData.threshold;

        OnRenderAreaBorderReached.AddUniqueListener(GenerateTerrain);
    }

    private void GenerateTerrain(Vector2 center)
    {
        Debug.Log("Generating terrain");
    }
}
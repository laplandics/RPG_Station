using System.Collections.Generic;
using UnityEngine;
using static GameDataInjector;
using static MapHelper;

public class TypeMapGenerator
{
    private static readonly int TypeMap = Shader.PropertyToID("_TypeMap");

    private readonly Dictionary<Vector2Int, Color> _tileTypePairs = new();
    private readonly Color[] _typeMapData;
    private readonly List<Tile> _currentTilesInVisibleArea = new();
    private Vector2Int _playerCurrentChunkCenterWorldPosition;
    private readonly Texture2D _typeMap;
    private readonly int _mapResolution;
    private readonly int _visibleArea;
    
    public TypeMapGenerator()
    {
        var mapData = InjectMapData;
        var chunkSize = mapData.chunkSize;
        _visibleArea = mapData.visibleChunks;
        _mapResolution = chunkSize * _visibleArea;
        _typeMap = new Texture2D(_mapResolution, _mapResolution, TextureFormat.RHalf, false);
        _typeMap.wrapMode = TextureWrapMode.Clamp;
        _typeMapData = new Color[_mapResolution * _mapResolution];
        Shader.SetGlobalTexture(TypeMap, _typeMap);
    }

    public void GenerateTypeMap(Vector2Int playerChunkIndex)
    {
        AssignVisibleArea(playerChunkIndex);
        UpdateTileColors();
        FillTypeMapData();
        ApplyMap();
    }

    private void AssignVisibleArea(Vector2Int chunkIndex)
    {
        _currentTilesInVisibleArea.Clear();
        _playerCurrentChunkCenterWorldPosition = GetChunkCenterWorldPosition(chunkIndex);
        var chunkIndexesInArea = GetChunksIndexesInArea(_visibleArea, chunkIndex);
        var chunks = GetChunksFromIndexes(chunkIndexesInArea);
        foreach (var chunk in chunks) _currentTilesInVisibleArea.AddRange(chunk.TilesData.Values);
    }
    
    private void UpdateTileColors()
    {
        foreach (var tile in _currentTilesInVisibleArea)
        {
            if (_tileTypePairs.ContainsKey(tile.Position)) continue;
            var index = TerrainTypePairs.TerrainTypeDictionary.GetValueOrDefault(tile.TerrainType, 0);
            var tileIndexColor = new Color(index, 0, 0, 1);
            _tileTypePairs.TryAdd(tile.Position, tileIndexColor);
        }
    }

    private void FillTypeMapData()
    {
        for (var x = 0; x < _mapResolution; x++)
        {
            for (var y = 0; y < _mapResolution; y++)
            {
                var worldX = _playerCurrentChunkCenterWorldPosition.x - _mapResolution / 2 + x;
                var worldY = _playerCurrentChunkCenterWorldPosition.y - _mapResolution / 2 + y;
                var worldPosition = new Vector2Int(worldX, worldY);
                _tileTypePairs.TryGetValue(worldPosition, out var color);
                _typeMapData[x + y * _mapResolution] = color;
            }
        }
    }

    private void ApplyMap()
    {
        _typeMap.SetPixels(_typeMapData);
        _typeMap.Apply();
        Shader.SetGlobalTexture(TypeMap, _typeMap);
    }
}
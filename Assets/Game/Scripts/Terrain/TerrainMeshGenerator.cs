using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static EventManager;
using static GlobalMapMethods;
using static TilesDataGenerator;
using static MemorizedAreaHandler;
using static GameDataInjector;

public class TerrainMeshGenerator : IDisposable
{
    private readonly Dictionary<Vector2Int, Chunk> _chunks = new();
    private readonly MapData _mapData;
    private readonly MapMajorSettingsSo _mapSettings;
    private readonly Transform _mapTransform;
    private readonly GOService _goService;
    private readonly Material _tileMaterial;
    private readonly Chunk _chunkPrefab;
    private readonly int _renderChunksCount;
    private readonly int _chunkSize;
    private readonly int _atlasColumns;
    private readonly int _atlasRows;
    private bool _isFirstGeneration;

    public TerrainMeshGenerator(Map map)
    {
        _goService = DS.GetSceneManager<GOService>();
        _mapTransform = map.transform;
        var mapData = InjectMapData;
        _mapSettings = InjectMapSettings;
        _chunkPrefab = _mapSettings.chunkPrefab;
        _renderChunksCount = mapData.renderChunksCount;
        _chunkSize = mapData.chunkSize;
        _atlasColumns = mapData.atlasColumns;
        _atlasRows = mapData.atlasRows;
        var atlas = _mapSettings.atlasTexture;
        _tileMaterial = new Material(Shader.Find("Sprites/Default"));
        _tileMaterial.mainTexture = atlas;
        _tileMaterial.hideFlags = HideFlags.DontSave;
        _isFirstGeneration = true;
        OnSmbEnteredChunk.AddListener(BuildNewTerrain);
        RestoreMemorizedArea(_mapSettings.MemorizedChunks, index => GenerateChunk(index), mapData.memorizedArea);
    }
    
    private void BuildNewTerrain(Vector2Int chunkIndex, IWalkable entity)
    {
        if (entity is not PlayerController) return;
        var indexes = GetChunksIndexesInArea(_renderChunksCount, chunkIndex);
        var nearChunks = new List<Vector2Int>();
        foreach (var index in indexes)
        {
            GenerateChunk(index, nearChunks);
            UpdateMemorizedArea(_mapSettings.MemorizedChunks, index);
        }
        ClearFarChunks(nearChunks);
        if (_isFirstGeneration) OnSceneReady?.Invoke();
        _isFirstGeneration = false;
    }
    
    private void GenerateChunk(Vector2Int index, List<Vector2Int> nearChunks = null)
    {
        nearChunks?.Add(index);
        if (_chunks.ContainsKey(index)) return;
        CreateChunk(index, GenerateTiles(index));
    }

    private void ClearFarChunks(List<Vector2Int> nearChunks)
    {
        var farChunks = new List<Vector2Int>();
        foreach (var chunk in _chunks)
        {
            if (_mapSettings.MemorizedChunks.Contains(chunk.Key)) continue;
            if (nearChunks.Contains(chunk.Key)) continue;
            farChunks.Add(chunk.Key);
            _goService.Despawn(chunk.Value.gameObject);
        }
        foreach (var farChunk in farChunks) { _chunks.Remove(farChunk); }
    }

    private void CreateChunk(Vector2Int position, Dictionary<Vector2Int, int> tiles)
    {
        var startTileX = position.x * _chunkSize;
        var startTileY = position.y * _chunkSize;
        var tileCount = _chunkSize * _chunkSize;
        var verts = new Vector3[tileCount * 4];
        var uvs = new Vector2[tileCount * 4];
        var tris = new int[tileCount * 6];
        var uvX = 1f / _atlasColumns;
        var uvY = 1f / _atlasRows;
        var v = 0;
        var t = 0;
        for (var y = 0; y < _chunkSize; y++)
        {
            for (var x = 0; x < _chunkSize; x++)
            {
                var globalTileX = startTileX + x;
                var globalTileY = startTileY + y;
                var tileIndex = tiles[new Vector2Int(globalTileX, globalTileY)];
                var vertexPositionX = x * Grid.TileSize;
                var vertexPositionY = y * Grid.TileSize;
                var vertexIndex = v;
                verts[vertexIndex + 0] = new Vector3(vertexPositionX, vertexPositionY, 0);
                verts[vertexIndex + 1] = new Vector3(vertexPositionX + Grid.TileSize, vertexPositionY, 0);
                verts[vertexIndex + 2] = new Vector3(vertexPositionX, vertexPositionY + Grid.TileSize, 0);
                verts[vertexIndex + 3] = new Vector3(vertexPositionX + Grid.TileSize, vertexPositionY + Grid.TileSize, 0);
                var texturePositionX = tileIndex % _atlasColumns;
                var texturePositionY = tileIndex / _atlasColumns;
                var uMin = texturePositionX * uvX;
                var vMin = texturePositionY * uvY;
                uvs[vertexIndex + 0] = new Vector2(uMin, vMin);
                uvs[vertexIndex + 1] = new Vector2(uMin + uvX, vMin);
                uvs[vertexIndex + 2] = new Vector2(uMin, vMin + uvY);
                uvs[vertexIndex + 3] = new Vector2(uMin + uvX, vMin + uvY);
                tris[t + 0] = vertexIndex + 0;
                tris[t + 1] = vertexIndex + 2;
                tris[t + 2] = vertexIndex + 1;
                tris[t + 3] = vertexIndex + 2;
                tris[t + 4] = vertexIndex + 3;
                tris[t + 5] = vertexIndex + 1;
                v += 4;
                t += 6;
            }
        }
        var mesh = new Mesh();
        if (verts.Length > 65000) mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.MarkDynamic();
        var chunkPosition = new Vector3(startTileX * Grid.TileSize, startTileY * Grid.TileSize, 1f);
        var chunk = _goService.Spawn(_chunkPrefab, chunkPosition, Quaternion.identity, _mapTransform);
        chunk.gameObject.name = $"Chunk {position.x}:{position.y}";
        chunk.Initialize(_tileMaterial, mesh, position, tiles);
        _chunks.TryAdd(position, chunk);
    }

    public void Dispose()
    {
        OnSmbEnteredChunk.RemoveListener(BuildNewTerrain);
        foreach (var kvp in _chunks) { if (kvp.Value != null) _goService.Despawn(kvp.Value.gameObject); }
        _chunks.Clear();
    }
}
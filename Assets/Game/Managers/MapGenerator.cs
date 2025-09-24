using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour, IInSceneManager
{
    [SerializeField] private int initializationOrder;
    [SerializeField][Range(1000, 9999)] private int seed;
    [SerializeField][Range(0.01f, 10f)] private float multX;
    [SerializeField][Range(0.01f, 10f)] private float multY;
    [SerializeField][Range(0f, 0.1f)] private float scale;
    [SerializeField] private int threshold;
    private EventManagerSo _eventManager;
    private Map _map;
    private Dictionary<Vector2Int, TerrainType> _terrains = new();

    private int _mapSize;
    public int InitializeOrder => initializationOrder;

    public void Initialize()
    {
        _eventManager = DS.GetSoManager<EventManagerSo>();

        _eventManager.onMapSpawned.AddListener(GenerateMapFile);
    }

    private void GenerateMapFile(Map map)
    {
        _map = map;
        _mapSize = _map.PrerenderMapSize;
        _map.MapData = new MapData();

        var positions = GetBoundaries();
        foreach (var position in positions)
        {
            var noisePosX = position.x / _map.ChunkSize;
            var noisePosY = position.y / _map.ChunkSize;
            var noise = GetPerlinNoise(noisePosX, noisePosY);
            var chunkTerrain = ChooseChunk(noise);
            _terrains.TryAdd(position, chunkTerrain);
        }
        for (var i = 0; i < 3; i++)
        {
            _terrains = ClearChunks();
        }
        var chunksData = GenerateMapDataFile(_terrains);
        _terrains.Clear();

        _map.MapData.savedChunksData = chunksData;
        _eventManager.onMapGenerated?.Invoke(_map);
    }

    private List<Vector2Int> GetBoundaries()
    {
        var boundaries = new List<Vector2Int>();
        var center = Vector2Int.zero;
        for (var y = -_mapSize; y <= _mapSize; y++)
        {
            for (var x = -_mapSize; x <= _mapSize; x++)
            {
                var chunkX = center.x + x;
                var chunkY = center.y + y;
                boundaries.Add(new Vector2Int(chunkX * _map.ChunkSize, chunkY * _map.ChunkSize));
            }
        }
        return boundaries;
    }

    private float GetPerlinNoise(int posX, int posY)
    {
        var noiseX = posX * scale + seed * multX;
        var noiseY = posY * scale + seed * multY;
        var noise = Mathf.PerlinNoise(noiseX, noiseY);

        return noise;
    }

    private TerrainType ChooseChunk(float noise)
    {
        var type = TerrainType.Water;
        foreach (var chunkPrefab in _map.TerrainChunks)
        {
            if (chunkPrefab is not ITerrainChunk terrain) continue;
            if (noise > terrain.NoiseMax || noise < terrain.NoiseMin) continue;
            type = terrain.TerrainType;
        }
        return type;
    }

    private Dictionary<Vector2Int, TerrainType> ClearChunks()
    {
        var newMap = new Dictionary<Vector2Int, TerrainType>(_terrains);
        foreach (var kvp in _terrains)
        {
            var pos = kvp.Key;
            var current = kvp.Value;

            var counts = new Dictionary<TerrainType, int>();

            var directions = new Vector2Int[]
            {
                new(-1, -1),
                new(0, -1),
                new(1, -1),
                new(-1, 0),
                new(1, 0),
                new(-1, 1),
                new(0, 1),
                new(1, 1)
            };

            foreach (var dir in directions)
            {
                var neighborPos = pos + dir * _map.ChunkSize;
                if (_terrains.TryGetValue(neighborPos, out var neighborType))
                {
                    if (!counts.ContainsKey(neighborType)) counts.TryAdd(neighborType, 0);
                    counts[neighborType]++;
                }
            }

            var majority = current;
            var maxCount = 0;

            foreach (var count in counts)
            {
                if (count.Value > maxCount)
                {
                    maxCount = count.Value;
                    majority = count.Key;
                }
            }

            if (majority != current && maxCount >= threshold) newMap[pos] = majority;
        }

        return newMap;
    }

    private List<ChunkData> GenerateMapDataFile(Dictionary<Vector2Int, TerrainType> terrain)
    {
        var data = new List<ChunkData>();
        foreach (var kvp in terrain)
        {
            foreach (var prefab in _map.TerrainChunks)
            {
                if (prefab is not ITerrainChunk terrainChunk) continue;
                if (terrainChunk.TerrainType != kvp.Value) continue;
                var chunkData = new ChunkData
                {
                    prefabKeyData = prefab.InstanceKey,
                    noiseData = GetPerlinNoise(kvp.Key.x / _map.ChunkSize, kvp.Key.y / _map.ChunkSize),
                    positionData = kvp.Key
                };
                data.Add(chunkData);
            }
        }

        return data;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "MapManager", menuName = "ManagersSO/MapManager")]
public class MapManagerSo : ScriptableObject, IObjectManagerSo
{
    public const string MAP_KEY = "MAP";
    public Dictionary<Vector2Int, TerrainType> terrain = new();
    public Dictionary<Vector2Int, Chunk> sceneChunks = new();
    [SerializeField] private MapDataStorage mapDataStorage;
    [SerializeField] private ChunksSpawnManagerSo _chunksSpawner;
    private TerrainDataGenerator _terrainDataGenerator;
    private Map _map;

    public MapData MapData { get; set; }
    public string Key => MAP_KEY;

    public SaveData Initialize()
    {
        MapData ??= NewMapData();

        _terrainDataGenerator = new TerrainDataGenerator();

        onMapSpawned.AddUniqueListener(SetMap);
        onPlayerSpawned.AddUniqueListener(SetPlayer);
        onPlayersPositionChanged.AddUniqueListener(UpdatePlayer);

        return MapData;
    }

    private void SetPlayer(Player player, SaveData _) => GenerateMap(player.transform);
    private void UpdatePlayer(Transform player) => GenerateMap(player);
    private MapData NewMapData()
    {
        var data = new MapData
        {
            instanceKey = MAP_KEY,
            chunkSize = mapDataStorage.chunkSize,
            mapSize = mapDataStorage.mapSize,
            renderAreaSize = mapDataStorage.renderAreaSize,
            seed = mapDataStorage.seed,
            scale = mapDataStorage.scale,
            multX = mapDataStorage.multX,
            multY = mapDataStorage.multY,
            threshold = mapDataStorage.threshold
        };

        return data;
    }

    private void SetMap(Map map, SaveData data)
    {
        if (data is not MapData mapData) return;
        MapData = mapData;
        _map = map;
        _chunksSpawner.InitializeSpawner(this, MapData, mapDataStorage);
        _terrainDataGenerator.Initialize(this, MapData, mapDataStorage);
    }

    private void GenerateMap(Transform player)
    {
        _chunksSpawner.GenerateChunksAroundPlayer(player, _map.transform);
    }

    public Vector2Int GetPlayersChunk(Transform playerTransform)
    {
        var player = playerTransform.position;
        var playerChunkX = (int)Math.Round(player.x / MapData.chunkSize);
        var playerChunkY = (int)Math.Round(player.y / MapData.chunkSize);

        return new Vector2Int(playerChunkX, playerChunkY);
    }

    public List<Vector2Int> GetBoundaries(int area, Vector2Int center)
    {
        var boundaries = new List<Vector2Int>();
        for (var y = -area; y <= area; y++)
        {
            for (var x = -area; x <= area; x++)
            {
                var chunkX = center.x + x;
                var chunkY = center.y + y;
                boundaries.Add(new Vector2Int(chunkX * MapData.chunkSize, chunkY * MapData.chunkSize));
            }
        }
        return boundaries;
    }

    public void SetNewData(SaveData newData)
    {
        if (newData is not MapData mapData) return;
        MapData = mapData;
    }

    public SaveData GetCurrentData() => MapData;

    public void DestroyCurrentInstance()
    {
        foreach (var chunk in sceneChunks)
        {
            Destroy(chunk.Value.gameObject);
        }
        sceneChunks.Clear();
        terrain.Clear();
        Destroy(_map.gameObject);
    }
}

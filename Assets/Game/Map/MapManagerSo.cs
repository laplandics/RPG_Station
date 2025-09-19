using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MapManager", menuName = "ManagersSO/MapManager")]
public class MapManagerSo : ScriptableObject
{
    [SerializeField] private MainGameObjectsSo instances;
    [SerializeField] private List<Chunk> allowedChunks;
    public List<Chunk> MapChunks { get => allowedChunks; }

    public Map Map { get; private set; }

    public Task Initialize()
    {
        DS.GetSoManager<EventManagerSo>().onMapSpawned.AddListener(InitializeMap);
        return Task.CompletedTask;
    }

    private void InitializeMap(Map map)
    {
        Map = map;
        Map.Initialize();
    }

    public async Task LoadMapData()
    {
        await Map.Load();
    }

    public async Task SaveMapData()
    {
        await Map.Save();
    }
}

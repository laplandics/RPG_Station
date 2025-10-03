using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "MapSpawner", menuName = "SpawnersSO/MapSpawner")]
public class MapSpawnerSo : ScriptableObject, ISpawner
{
    [SerializeField] private Map mapPrefab;
    private MapData _mapData;
    private AllTerrainsData _allTerrainsData;

    public void InitializeSpawner()
    {
        _mapData = MapDataHandler.GetMapData;
        _allTerrainsData = TerrainDataHandler.GetAllTerrainsData;
        SpawnMap();
    }

    private void SpawnMap()
    {
        var map = Instantiate(mapPrefab, Vector2.zero, Quaternion.identity);
        map.gameObject.name = _mapData.instanceKey;
        OnMapSpawned?.Invoke(map, _mapData, _allTerrainsData);
    }
}

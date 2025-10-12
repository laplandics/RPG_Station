using UnityEngine;
using static EventManager;
using static GameDataInjector;

[CreateAssetMenu(fileName = "MapSpawner", menuName = "SpawnersSO/MapSpawner")]
public class MapSpawnerSo : ScriptableObject, ISpawner
{
    [SerializeField] private Map mapPrefab;
    private MapData _mapData;
    private AllTilesData _allTilesData;
    private AllTerrainData _allTerrainData;

    public void InitializeSpawner()
    {
        _mapData = InjectMapData;
        _allTilesData = InjectTilesData;
        _allTerrainData = InjectTerrainData;
        SpawnMap();
    }

    private void SpawnMap()
    {
        var map = Instantiate(mapPrefab, Vector2.zero, Quaternion.identity);
        map.gameObject.name = _mapData.instanceKey;
        OnMapSpawned?.Invoke(map, _mapData, _allTilesData, _allTerrainData);
    }
}

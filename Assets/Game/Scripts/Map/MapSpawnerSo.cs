using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "MapSpawner", menuName = "SpawnersSO/MapSpawner")]
public class MapSpawnerSo : ScriptableObject, ISpawner
{
    [SerializeField] private Map mapPrefab;
    private MapData _mapData;

    public void InitializeSpawner()
    {
        _mapData = SaveDataService.GetMapData;
        SpawnMap();
    }

    private void SpawnMap()
    {
        var map = Instantiate(mapPrefab, Vector2.zero, Quaternion.identity);
        map.gameObject.name = _mapData.instanceKey;
        OnMapSpawned?.Invoke(map, _mapData);
    }
}

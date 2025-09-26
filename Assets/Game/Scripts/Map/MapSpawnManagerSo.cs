using System.Collections;
using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "MapSpawner", menuName = "SpawnersSO/MapSpawner")]
public class MapSpawnManagerSo : ScriptableObject, ISpawner
{
    [SerializeField] private Map _mapPrefab;
    private MapData _mapData;

    public bool TryInitializeSpawner(SaveData data)
    {
        if (data is not MapData mapData) return false;
        _mapData = mapData;
        DS.GetSceneManager<RoutineManager>().StartRoutine(SpawnMap());
        return true;
    }

    private IEnumerator SpawnMap()
    {
        var map = Instantiate(_mapPrefab, Vector2.zero, Quaternion.identity);
        map.gameObject.name = _mapData.instanceKey;
        onMapSpawned?.Invoke(map, _mapData);
        yield break;
    }
}

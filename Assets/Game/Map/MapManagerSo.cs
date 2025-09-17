using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MapManager", menuName = "ManagersSO/MapManager")]
public class MapManagerSo : ScriptableObject
{
    [SerializeField] private InitInstances instances;

    public Map Map { get; private set; }

    public Task<Map> SpawnMap()
    {
        Map = Instantiate(instances.mapPrefab, instances.mapPrefab.transform.position, Quaternion.identity);
        Map.Initialize();
        return Task.FromResult(Map);
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

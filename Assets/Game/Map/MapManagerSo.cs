using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MapManager", menuName = "ManagersSO/MapManager")]
public class MapManagerSo : ScriptableObject
{
    [SerializeField] private InitInstances instances;
    private Map _map;
    
    public Task<Map> SpawnMap()
    {
        _map = Instantiate(instances.mapPrefab, instances.mapPrefab.transform.position, Quaternion.identity).GetComponent<Map>();
        return Task.FromResult(_map);
    }
}

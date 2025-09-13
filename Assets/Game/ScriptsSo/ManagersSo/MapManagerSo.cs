using UnityEngine;

[CreateAssetMenu(fileName = "MapManager", menuName = "ManagersSO/MapManager")]
public class MapManagerSo : ScriptableObject
{
    private GameObject _map;
    
    public GameObject SpawnMap(GameObject mapPrefab)
    {
        _map = Instantiate(mapPrefab, mapPrefab.transform.position, Quaternion.identity);
        return _map;
    }
}
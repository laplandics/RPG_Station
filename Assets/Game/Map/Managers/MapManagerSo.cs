using UnityEngine;

[CreateAssetMenu(fileName = "MapManager", menuName = "ManagersSO/MapManager")]
public class MapManagerSo : ScriptableObject
{
    public Map Map { get; private set; }

    public void Initialize()
    {
        DS.GetSoManager<EventManagerSo>().onMapSpawned.AddListener(InitializeMap);
    }

    private void InitializeMap(Map map)
    {
        Map = map;
        Map.Initialize();
    }

    public void LoadMapData() => Map.Load();

    public void SaveMapData() => Map.Save();
}

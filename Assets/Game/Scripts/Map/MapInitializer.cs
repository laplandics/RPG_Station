using static EventManager;

public class MapInitializer
{
    private readonly GOService _goService;
    private Map _map;
    
    public MapData CurrentMapData { get; private set; }
    public AllTilesData CurrentAllTilesData { get; private set; }
    public AllBiomesData CurrentAllBiomesData { get; private set; }
    public TerrainMeshGenerator TerrainMeshGenerator {get; private set;}

    public MapInitializer()
    {
        _goService = DS.GetSceneManager<GOService>();
        OnMapSpawned.AddListener(SetMap);
    }

    private void SetMap(Map map, MapData data, AllTilesData allTilesData, AllBiomesData biomesData)
    {
        CurrentMapData = data;
        CurrentAllTilesData = allTilesData;
        CurrentAllBiomesData = biomesData;
        _map = map;
        TerrainMeshGenerator = new TerrainMeshGenerator(map);
    }
    
    public void DeInitialize()
    {
        TerrainMeshGenerator.Dispose();
        TerrainMeshGenerator = null;
        _goService.Despawn(_map.gameObject);
        CurrentMapData = null;
        CurrentAllTilesData = null;
        OnMapSpawned.RemoveListener(SetMap);
    }
}

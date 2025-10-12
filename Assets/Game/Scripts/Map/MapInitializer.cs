using static EventManager;

public class MapInitializer
{
    private readonly GOService _goService;
    private Map _map;
    
    public MapData CurrentMapData { get; private set; }
    public AllTilesData CurrentAllTilesData { get; private set; }
    public AllTerrainData CurrentAllTerrainData { get; private set; }
    public TerrainGenerator TerrainGenerator {get; private set;}

    public MapInitializer()
    {
        _goService = DS.GetSceneManager<GOService>();
        OnMapSpawned.AddListener(SetMap);
    }

    private void SetMap(Map map, MapData data, AllTilesData allTilesData, AllTerrainData terrainData)
    {
        CurrentMapData = data;
        CurrentAllTilesData = allTilesData;
        CurrentAllTerrainData = terrainData;
        _map = map;
        TerrainGenerator = new TerrainGenerator(map);
    }
    
    public void DeInitialize()
    {
        TerrainGenerator.Dispose();
        TerrainGenerator = null;
        _goService.Despawn(_map.gameObject);
        CurrentMapData = null;
        CurrentAllTilesData = null;
        OnMapSpawned.RemoveListener(SetMap);
    }
}

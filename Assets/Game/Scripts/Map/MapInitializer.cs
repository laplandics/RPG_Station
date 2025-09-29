using static EventManager;

public class MapInitializer
{
    private readonly GOService _goService;
    private Map _map;
    
    public MapData CurrentMapData { get; private set; }
    public TerrainsData CurrentTerrainsData { get; private set; }
    public TerrainDataGenerator TerrainDataGenerator {get ; private set;}
    public TerrainChunksGenerator TerrainChunksGenerator {get; private set;}

    public MapInitializer()
    {
        _goService = DS.GetSceneManager<GOService>();
        OnMapSpawned.AddListener(SetMap);
    }

    private void SetMap(Map map, MapData data, TerrainsData terrainsData)
    {
        CurrentMapData = data;
        CurrentTerrainsData = terrainsData;
        _map = map;
        
        TerrainDataGenerator = new TerrainDataGenerator(data);
        TerrainChunksGenerator = new TerrainChunksGenerator(this, map, data);
    }
    
    public void DeInitialize()
    {
        TerrainDataGenerator.Dispose();
        TerrainChunksGenerator.Dispose();
        _goService.Despawn(_map.gameObject);
        CurrentMapData = null;
        CurrentTerrainsData = null;
        OnMapSpawned.RemoveListener(SetMap);
    }
}

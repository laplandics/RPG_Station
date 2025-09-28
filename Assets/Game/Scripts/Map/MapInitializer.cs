using static EventManager;

public class MapInitializer
{
    private readonly GameObjectsService _gameObjectsService;
    private TerrainDataGenerator _terrainDataGenerator;
    private ChunksGenerator _chunksGenerator;
    private Map _map;

    public MapInitializer()
    {
        _gameObjectsService = DS.GetSceneManager<GameObjectsService>();
        OnMapSpawned.AddListener(SetMap);
    }

    private void SetMap(Map map, MapData data)
    {
        _map = map;
        _terrainDataGenerator = new TerrainDataGenerator(data);
        _chunksGenerator = new ChunksGenerator(map, data);
    }
    
    public void DeInitialize()
    {
        
    }
}

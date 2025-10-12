public static class GameDataInjector
{
    public static MapData InjectMapData => DS.GetGlobalManager<GlobalDataManagerSo>().GetDataHandler<MapDataHandler>().GetData<MapData>();
    
    public static MapMajorSettingsSo InjectMapSettings => DS.GetGlobalManager<GlobalDataManagerSo>().GetObjectSettings<MapMajorSettingsSo>();
    
    public static PlayerData InjectPlayerData => DS.GetGlobalManager<GlobalDataManagerSo>().GetDataHandler<PlayerDataHandler>().GetData<PlayerData>();
    
    public static AllTilesData InjectTilesData => DS.GetGlobalManager<GlobalDataManagerSo>().GetDataHandler<TilesDataHandler>().GetData<AllTilesData>();
    
    public static AllTilesMajorSettingsSo InjectTilesSettings => DS.GetGlobalManager<GlobalDataManagerSo>().GetObjectSettings<AllTilesMajorSettingsSo>();
    
    public static AllTerrainData InjectTerrainData => DS.GetGlobalManager<GlobalDataManagerSo>().GetDataHandler<TerrainDataHandler>().GetData<AllTerrainData>();
    
    public static AllTerrainTypesMajorSettingsSo InjectTerrainTypesSettings = DS.GetGlobalManager<GlobalDataManagerSo>().GetObjectSettings<AllTerrainTypesMajorSettingsSo>();
}
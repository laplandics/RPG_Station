public static class GameDataInjector
{
    public static MapData InjectMapData => DS.GetGlobalManager<GlobalDataManagerSo>().GetDataHandler<MapDataHandler>().GetData<MapData>();
    
    public static MapMajorSettingsSo InjectMapSettings => DS.GetGlobalManager<GlobalDataManagerSo>().GetObjectSettings<MapMajorSettingsSo>();
    
    public static PlayerData InjectPlayerData => DS.GetGlobalManager<GlobalDataManagerSo>().GetDataHandler<PlayerDataHandler>().GetData<PlayerData>();
    
    public static AllTilesData InjectTilesData => DS.GetGlobalManager<GlobalDataManagerSo>().GetDataHandler<TilesDataHandler>().GetData<AllTilesData>();
    
    public static AllTilesMajorSettingsSo InjectTilesSettings => DS.GetGlobalManager<GlobalDataManagerSo>().GetObjectSettings<AllTilesMajorSettingsSo>();
    
    public static AllBiomesData InjectBiomesData => DS.GetGlobalManager<GlobalDataManagerSo>().GetDataHandler<BiomesDataHandler>().GetData<AllBiomesData>();
    
    public static AllBiomesMajorSettingsSo InjectBiomesSettings = DS.GetGlobalManager<GlobalDataManagerSo>().GetObjectSettings<AllBiomesMajorSettingsSo>();
}
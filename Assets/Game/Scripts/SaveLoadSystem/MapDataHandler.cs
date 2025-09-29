public static class MapDataHandler
{
    public static MapData GetMapData { get; private set; }
    public static MapSettingsSo GetMapSettingsSo { get; private set; }
    public static void SetMapSettings(MapSettingsSo ms)
    {
        GetMapSettingsSo = ms;
        GetMapData = new MapData
        {
            instanceKey = ms.mapKey,
            chunkSize = ms.chunkSize,
            mapSize = ms.mapSize,
            renderAreaSize = ms.renderAreaSize,
            seed = ms.seed,
            scale = ms.scale,
            multX = ms.multX,
            multY = ms.multY,
            threshold = ms.threshold
        };
    }

    public static MapData SetMapData(MapData mapData = null)
    {
        mapData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentMapData;
        GetMapData = mapData;
        
        GetMapSettingsSo.mapKey = mapData.instanceKey;
        GetMapSettingsSo.chunkSize = mapData.chunkSize;
        GetMapSettingsSo.mapSize = mapData.mapSize;
        GetMapSettingsSo.renderAreaSize = mapData.renderAreaSize;
        GetMapSettingsSo.seed = mapData.seed;
        GetMapSettingsSo.scale = mapData.scale;
        GetMapSettingsSo.multX = mapData.multX;
        GetMapSettingsSo.multY = mapData.multY;
        GetMapSettingsSo.threshold = mapData.threshold;

        return GetMapData;
    }
}
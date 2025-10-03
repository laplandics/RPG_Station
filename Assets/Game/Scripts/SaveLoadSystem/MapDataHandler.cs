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
            renderChunksCount = ms.renderChunksCount,
            memorizedArea = ms.memorizedArea,
            tilesCalculationArea = ms.tilesCalculationArea,
            seed = ms.seed,
            scale = ms.scale,
            octaves = ms.octaves,
            persistence = ms.persistence,
            lacunarity = ms.lacunarity,
            atlasColumns = ms.atlasColumns,
            atlasRows = ms.atlasRows,
            ChunksToRemember = ms.MemorizedChunks
        };
    }

    public static MapData SetMapData(MapData mapData = null)
    {
        mapData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentMapData;
        GetMapData = mapData;
        
        GetMapSettingsSo.mapKey = mapData.instanceKey;
        GetMapSettingsSo.chunkSize = mapData.chunkSize;
        GetMapSettingsSo.renderChunksCount = mapData.renderChunksCount;
        GetMapSettingsSo.memorizedArea = mapData.memorizedArea;
        GetMapSettingsSo.tilesCalculationArea = mapData.tilesCalculationArea;
        GetMapSettingsSo.seed = mapData.seed;
        GetMapSettingsSo.scale = mapData.scale;
        GetMapSettingsSo.octaves = mapData.octaves;
        GetMapSettingsSo.persistence = mapData.persistence;
        GetMapSettingsSo.lacunarity = mapData.lacunarity;
        GetMapSettingsSo.atlasColumns = mapData.atlasColumns;
        GetMapSettingsSo.atlasRows = mapData.atlasRows;
        GetMapSettingsSo.MemorizedChunks = mapData.ChunksToRemember;

        return GetMapData;
    }
}
using UnityEngine;

public class MapDataHandler : IDataHandler
{
    private MapData _mapData;
    private MapMajorSettingsSo _mapSettings;

    public T GetData<T>() where T : SaveData => _mapData as T;
    public T GetSettings<T>() where T : ScriptableObject => _mapSettings as T;
    
    public bool TrySetSettings(IMajorSettings majorSettings)
    {
        if (majorSettings is not MapMajorSettingsSo ms) return false;
        _mapSettings = ms;
        _mapData = new MapData
        {
            instanceKey = ms.InstanceKey,
            seed = ms.seed,
            chunkSize = ms.chunkSize,
            renderChunksCount = ms.renderChunksCount,
            memorizedArea = ms.memorizedArea,
            tilesCalculationArea = ms.tilesCalculationArea,
            defaultTerrainType = ms.defaultTerrainType,
            visibleChunks = ms.visibleChunks,
            ChunksToRemember = ms.MemorizedChunks,
            columns = ms.columns,
            rows = ms.rows
        };
        return true;
    }

    public SaveData SendReceiveData(SaveData data = null)
    {
        var mapData = data as MapData;
        mapData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentMapData;
        _mapData = mapData;
        
        _mapSettings.InstanceKey = mapData.instanceKey;
        _mapSettings.seed = mapData.seed;
        _mapSettings.chunkSize = mapData.chunkSize;
        _mapSettings.renderChunksCount = mapData.renderChunksCount;
        _mapSettings.memorizedArea = mapData.memorizedArea;
        _mapSettings.tilesCalculationArea = mapData.tilesCalculationArea;
        _mapSettings.visibleChunks = mapData.visibleChunks;
        _mapSettings.defaultTerrainType = mapData.defaultTerrainType;
        _mapSettings.MemorizedChunks = mapData.ChunksToRemember;
        _mapSettings.columns = mapData.columns;
        _mapSettings.rows = mapData.rows;

        return _mapData;
    }

    public bool TryLoadData()
    {
        var data = DS.GetGlobalManager<SaveLoadManagerSo>().Load<MapData>(_mapSettings.InstanceKey);
        return data != null && SendReceiveData(data) is MapData;
    }

    public bool TrySaveData()
    {
        var data = SendReceiveData();
        if (data is not MapData mapData) return false;
        DS.GetGlobalManager<SaveLoadManagerSo>().Save(mapData.instanceKey, mapData);
        return true;
    }
}
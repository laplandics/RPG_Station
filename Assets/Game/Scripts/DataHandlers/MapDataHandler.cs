using UnityEngine;

public class MapDataHandler : IDataHandler
{
    private MapData _mapData;
    private MapMajorSettingsSo _mapSettings;

    public T GetData<T>() where T : SaveData => _mapData as T;
    public T GetSettings<T>() where T : ScriptableObject  => _mapSettings as T;
    
    public bool TrySetSettings(IMajorSettings majorSettings)
    {
        if (majorSettings is not MapMajorSettingsSo ms) return false;
        _mapSettings = ms;
        _mapData = new MapData
        {
            instanceKey = ms.InstanceKey,
            chunkSize = ms.chunkSize,
            renderChunksCount = ms.renderChunksCount,
            memorizedArea = ms.memorizedArea,
            tilesCalculationArea = ms.tilesCalculationArea,
            atlasColumns = ms.atlasColumns,
            atlasRows = ms.atlasRows,
            ChunksToRemember = ms.MemorizedChunks
        };
        return true;
    }

    public SaveData SendReceiveData(SaveData data = null)
    {
        var mapData = data as MapData;
        mapData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentMapData;
        _mapData = mapData;
        
        _mapSettings.InstanceKey = mapData.instanceKey;
        _mapSettings.chunkSize = mapData.chunkSize;
        _mapSettings.renderChunksCount = mapData.renderChunksCount;
        _mapSettings.memorizedArea = mapData.memorizedArea;
        _mapSettings.tilesCalculationArea = mapData.tilesCalculationArea;
        _mapSettings.atlasColumns = mapData.atlasColumns;
        _mapSettings.atlasRows = mapData.atlasRows;
        _mapSettings.MemorizedChunks = mapData.ChunksToRemember;

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
using System.Collections.Generic;
using UnityEngine;

public class TilesDataHandler : IDataHandler
{

    private AllTilesData _tilesData;
    private AllTilesMajorSettingsSo _tilesSettings;
    
    public T GetData<T>() where T : SaveData => _tilesData as T;

    public T GetSettings<T>() where T : ScriptableObject => _tilesSettings as T;

    public bool TrySetSettings(IMajorSettings majorSettings)
    {
        if (majorSettings is not AllTilesMajorSettingsSo tileSettings) return false;
        _tilesSettings = tileSettings;
        _tilesData = new AllTilesData { instanceKey = _tilesSettings.InstanceKey, allTilesData = new List<TileData>() };
        foreach (var ts in _tilesSettings.tilesSettings)
        {
            _tilesData.allTilesData.Add
            (
                new TileData
                {
                    instanceKey = ts.tileKey,
                    noise = ts.noise,
                    tileAtlasIndexes = ts.tileAtlasIndexes,
                    isUnreachable = ts.isUnreachable
                }
            );
        }
        return true;
    }

    public SaveData SendReceiveData(SaveData data = null)
    {
        var allTilesData = data as AllTilesData;
        allTilesData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentAllTilesData;
        _tilesData = allTilesData;
        foreach (var tileSettingsSo in _tilesSettings.tilesSettings)
        {
            foreach (var tileData in _tilesData.allTilesData)
            {
                if (tileSettingsSo.tileKey != tileData.instanceKey) continue;
                tileSettingsSo.noise = tileData.noise;
                tileSettingsSo.tileAtlasIndexes = tileData.tileAtlasIndexes;
                tileSettingsSo.isUnreachable = tileData.isUnreachable;
                break;
            }
        }

        return _tilesData;
    }

    public bool TryLoadData()
    {
        var data = DS.GetGlobalManager<SaveLoadManagerSo>().Load<AllTilesData>(_tilesSettings.InstanceKey);
        return data != null && SendReceiveData(data) is AllTilesData;
    }

    public bool TrySaveData()
    {
        var data = SendReceiveData();
        if (data is not AllTilesData tilesData) return false;
        DS.GetGlobalManager<SaveLoadManagerSo>().Save(tilesData.instanceKey, tilesData);
        return true;
    }
}
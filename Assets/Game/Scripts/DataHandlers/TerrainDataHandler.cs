using System.Collections.Generic;
using UnityEngine;

public class TerrainDataHandler : IDataHandler
{
    private AllTerrainData _terrainData;
    private AllTerrainTypesMajorSettingsSo _terrainTypeSettings;

    public T GetData<T>() where T : SaveData => _terrainData as T;

    public T GetSettings<T>() where T : ScriptableObject => _terrainTypeSettings as T;

    public bool TrySetSettings(IMajorSettings majorSettings)
    {
        if (majorSettings is not AllTerrainTypesMajorSettingsSo terrainTypeSettings) return false;
        _terrainTypeSettings = terrainTypeSettings;
        _terrainData = new AllTerrainData
        {
            instanceKey = _terrainTypeSettings.InstanceKey,
            allTerrainData = new List<TerrainData>()
        };
        foreach (var bs in _terrainTypeSettings.terrainDataGenerators)
        {
            _terrainData.allTerrainData.Add
            (
                new TerrainData
                {
                    instanceKey = bs.terrainKey,
                    weight = bs.terrainWeight,
                }
            );
        }
        return true;
    }

    public SaveData SendReceiveData(SaveData saveData = null)
    {
        var allBiomesData = saveData as AllTerrainData;
        allBiomesData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentAllTerrainData;
        _terrainData = allBiomesData;
        
        _terrainTypeSettings.instanceKey = _terrainData.instanceKey;
        foreach (var settingsSo in _terrainTypeSettings.terrainDataGenerators)
        {
            foreach (var terrainData in _terrainData.allTerrainData)
            {
                if (settingsSo.terrainKey != terrainData.instanceKey) continue;
                settingsSo.terrainWeight = terrainData.weight;
                break;
            }
        }
        return _terrainData;
    }

    public bool TryLoadData()
    {
        var data = DS.GetGlobalManager<SaveLoadManagerSo>().Load<AllTerrainData>(_terrainTypeSettings.InstanceKey);
        return data != null && SendReceiveData(data) is AllTerrainData;
    }

    public bool TrySaveData()
    {
        var data = SendReceiveData();
        if (data is not AllTerrainData allTerrainData) return false;
        DS.GetGlobalManager<SaveLoadManagerSo>().Save(allTerrainData.instanceKey, allTerrainData);
        return true;
    }
}
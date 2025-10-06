using System.Collections.Generic;
using UnityEngine;

public class BiomesDataHandler : IDataHandler
{
    private AllBiomesData _biomesData;
    private AllBiomesMajorSettingsSo _biomeSettings;

    public T GetData<T>() where T : SaveData => _biomesData as T;

    public T GetSettings<T>() where T : ScriptableObject => _biomeSettings as T;

    public bool TrySetSettings(IMajorSettings majorSettings)
    {
        if (majorSettings is not AllBiomesMajorSettingsSo biomeSettings) return false;
        _biomeSettings = biomeSettings;
        _biomesData = new AllBiomesData
        {
            instanceKey = _biomeSettings.InstanceKey,
            biomesCount = _biomeSettings.BiomesCount,
            seed = _biomeSettings.seed,
            borderSize = _biomeSettings.borderSize,
            jitter = _biomeSettings.jitter,
            biomeSize = _biomeSettings.biomeSize,
            allBiomesData = new List<BiomeData>()
        };
        foreach (var bs in _biomeSettings.biomeSettings)
        {
            _biomesData.allBiomesData.Add
            (
                new BiomeData
                {
                    instanceKey = bs.biomeKey,
                    biomeType = bs.biomeType,
                    weight = bs.biomeWeight,
                }
            );
        }
        return true;
    }

    public SaveData SendReceiveData(SaveData saveData = null)
    {
        var allBiomesData = saveData as AllBiomesData;
        allBiomesData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentAllBiomesData;
        _biomesData = allBiomesData;
        
        _biomeSettings.instanceKey = _biomesData.instanceKey;
        _biomeSettings.seed = _biomesData.seed;
        _biomeSettings.borderSize = _biomesData.borderSize;
        _biomeSettings.jitter = _biomesData.jitter;
        _biomeSettings.biomeSize = _biomesData.biomeSize;
        foreach (var biomeSettingsSo in _biomeSettings.biomeSettings)
        {
            foreach (var biomeData in _biomesData.allBiomesData)
            {
                if (biomeSettingsSo.biomeKey != biomeData.instanceKey) continue;
                biomeSettingsSo.biomeType = biomeData.biomeType;
                biomeSettingsSo.biomeWeight = biomeData.weight;
                break;
            }
        }
        return _biomesData;
    }

    public bool TryLoadData()
    {
        var data = DS.GetGlobalManager<SaveLoadManagerSo>().Load<AllBiomesData>(_biomeSettings.InstanceKey);
        return data != null && SendReceiveData(data) is AllBiomesData;
    }

    public bool TrySaveData()
    {
        var data = SendReceiveData();
        if (data is not AllBiomesData biomesData) return false;
        DS.GetGlobalManager<SaveLoadManagerSo>().Save(biomesData.instanceKey, biomesData);
        return true;
    }
}
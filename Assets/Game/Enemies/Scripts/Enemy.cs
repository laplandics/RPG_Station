using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ISaveAble
{
    [SerializeField] ScriptableObject[] modsSetters;
    [SerializeField] private string prefabKey;
    private List<IEnemyMod> _mods = new();
    public EnemyData EnemyData { get; set; }
    public string InstanceKey { get => prefabKey; set => prefabKey = value; }

    public void Initialize()
    {
        foreach (var mod in modsSetters)
        {
            if (mod is not IEnemyModSetter setter) continue;
            _mods.Add(setter.SetMod(this));
        }
        foreach (var mod in _mods) {mod.LoadMod();}
    }

    public Task Load()
    {
        foreach (var modData in EnemyData.mods)
        {
            foreach (var mod in _mods)
            {
                if (mod is not ISaveAble saveMod) continue;
                if (modData.key != saveMod.InstanceKey) continue;
                saveMod.Load();
            }
        }
        return Task.CompletedTask;
    }

    public Task Save()
    {
        var modsData = new List<ModData>();
        foreach (var mod in _mods)
        {
            if (mod is not ISaveAble saveMod) continue;
            saveMod.Save();
            modsData.Add(mod.GetModData());
        }
        EnemyData = new EnemyData
        {
            prefabKey = prefabKey,
            position = transform.position,
            mods = modsData
        };

        return Task.CompletedTask;
    }
}

[Serializable]
public class EnemyData
{
    public string prefabKey;
    public Vector3 position;
    public List<ModData> mods; 
}

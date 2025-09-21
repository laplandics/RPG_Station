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
    public float BuferTime { get; set; }

    public void Initialize()
    {
        foreach (var mod in modsSetters)
        {
            if (mod is not IEnemyModSetter setter) continue;
            _mods.Add(setter.SetMod(this));
        }
        foreach (var mod in _mods) { mod.LoadMod(); }
    }

    public void Load()
    {
        foreach (var modData in EnemyData.mods)
        {
            foreach (var mod in _mods)
            {
                if (mod is not ISaveAble saveMod) continue;
                if (modData.key != saveMod.InstanceKey) continue;
                mod.SetModData(modData);
                saveMod.Load();
            }
        }
    }

    public void Save()
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
    }

    private void OnDestroy()
    {
        foreach (var mod in _mods)
        {
            mod.UnloadMod();
        }
    }
}

[Serializable]
public class EnemyData
{
    public string prefabKey;
    public Vector3 position;
    public List<ModData> mods; 
}

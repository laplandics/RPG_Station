using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMoveMod", menuName = "EnemyMods/EnemyMoveMod")]
public class EnemyMoveModSo : ScriptableObject, IEnemyModSetter
{
    [SerializeField] private string modKey;
    public string ModKey { get => modKey; set => modKey = value; } 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float routFindArea;

    public IEnemyMod SetMod(Enemy owner)
    {
        var mod = new MoveMod
        {
            key = modKey,
            moveSpeed = moveSpeed,
            routFindArea = routFindArea,
            Owner = owner,
            ModData = new ModData()
        };

        return mod;
    }
}

[Serializable]
public class MoveMod : IEnemyMod, ISaveAble
{
    public string key;
    public float moveSpeed;
    public float routFindArea;

    public Enemy Owner { get; set; }
    public string InstanceKey { get => key; set => key = value; }
    public ModData ModData { get; set; }

    public void LoadMod()
    {
        DS.GetSoManager<EventManagerSo>().onTimePassed.AddListener(time => DS.GetSceneManager<RoutineManager>().StartRoutine(move(time)));
    }

    public void UnLoadMod() { }

    public Task Save()
    {
        var data = new ModData
        {
            key = key,
            firstValue = moveSpeed,
            secondValue = routFindArea,
        };
        ModData = data;

        return Task.CompletedTask;
    }

    public Task Load()
    {
        var data = ModData;
        key = data.key;
        moveSpeed = data.firstValue;
        routFindArea = data.secondValue;

        return Task.CompletedTask;
    }

    private IEnumerator move(float time)
    {
        Debug.Log($"Enemy {Owner.gameObject.name} moved");
        yield break;
    }

    public ModData GetModData() => ModData;
}
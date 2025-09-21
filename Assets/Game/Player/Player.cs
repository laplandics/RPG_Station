using System;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour, ISaveAble
{
    [SerializeField] private string key;
    [SerializeField] private PlayerController controller;
    public PlayerController GetController() => controller;
    public string InstanceKey { get => key; set => key = value; }
    public async Task Save()
    {
        var data = new PlayerData
        {
            position = transform.position,
        };
        await DS.GetSoManager<SaveLoadManagerSo>().Save(key,data);
    }

    public async Task Load()
    {
        var data = await DS.GetSoManager<SaveLoadManagerSo>().Load<PlayerData>(key);
        transform.position = data.position;
        DS.GetSoManager<EventManagerSo>().onPlayersPositionChanged?.Invoke(transform);
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
}
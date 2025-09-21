using System;
using UnityEngine;

public class Player : MonoBehaviour, ISaveAble
{
    [SerializeField] private string key;
    [SerializeField] private PlayerController controller;
    public PlayerController GetController() => controller;
    public string InstanceKey { get => key; set => key = value; }
    public void Save()
    {
        var data = new PlayerData
        {
            position = transform.position,
        };
        DS.GetSoManager<SaveLoadManagerSo>().Save(key,data);
    }

    public void Load()
    {
        var data = DS.GetSoManager<SaveLoadManagerSo>().Load<PlayerData>(key);
        transform.position = data.position;
        DS.GetSoManager<EventManagerSo>().onPlayersPositionChanged?.Invoke(transform);
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
}
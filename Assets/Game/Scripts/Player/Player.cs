using System;
using UnityEngine;

public class Player : MonoBehaviour, ISaveAble
{
    [SerializeField] private string key;
    [SerializeField] private PlayerController controller;
    public PlayerController GetController() => controller;
    public string PrefabKey { get => key; set => key = value; }
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
        DS.GetSoManager<SaveLoadManagerSo>().Load<PlayerData>(key, data =>
        {
            transform.position = data.position;
        });
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
}
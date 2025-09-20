using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ISaveAble
{
    [SerializeField] private string prefabKey;
    public EnemyData EnemyData { get; set; }
    public string PrefabKey { get => prefabKey; set => prefabKey = value; }

    public Task Load()
    {
        return Task.CompletedTask;
    }

    public Task Save()
    {
        EnemyData = new EnemyData
        {
            prefabKey = prefabKey,
            position = transform.position
        };
        return Task.CompletedTask;
    }
}

[Serializable]
public class EnemyData
{
    public string prefabKey;
    public Vector3 position;
}

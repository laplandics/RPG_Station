using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ISaveAble
{
    [SerializeField] private string prefabKey;
    private Chunk _parentChunk;
    public string InstanceKey { get => prefabKey; set => prefabKey = value; }
    public EnemyData EnemyData { get; set; }

    public void Initialize(Chunk parentChunk)
    {
        _parentChunk = parentChunk;
    }

    public void Load()
    {
    }

    public void Save()
    {
        EnemyData = new EnemyData
        {
            prefabKeyData = prefabKey,
            positionData = transform.position,
            modsData = new List<ModData>()
        };
    }
}

[Serializable]
public class EnemyData
{
    public string prefabKeyData;
    public Vector2 positionData;
    public List<ModData> modsData; 
}

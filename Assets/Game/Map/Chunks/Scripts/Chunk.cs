using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chunk : MonoBehaviour, ISaveAble
{
    [Header("Saveload id")]
    [SerializeField] private string prefabKey;
    [Header("Chunk settings")]
    [SerializeField] private int minEnemies;
    [SerializeField] private int maxEnemies;
    [SerializeField] private Enemy[] allowedEnemies;
    [SerializeField] private Collider2D[] spawnZones;

    private EventManagerSo _eventManager;
    private EnemiesManagerSo _enemiesManager;
    private Vector2Int _intPosition;
    private bool _isVisited;

    public string InstanceKey { get => prefabKey; set => prefabKey = value; }
    public int MinEnemies => minEnemies;
    public int MaxEnemies => maxEnemies;
    public Enemy[] AllowedEnemies => allowedEnemies;
    public Collider2D[] SpawnZones => spawnZones;
    public ChunkData ChunkData { get; set; }

    public void SetChunk()
    {
        _eventManager = DS.GetSoManager<EventManagerSo>();
        _enemiesManager = DS.GetSoManager<EnemiesManagerSo>();
        _intPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        _isVisited = true;

        _eventManager.onChunkSpawned?.Invoke(_intPosition, this);
    }

    public void Load()
    {
        _enemiesManager.AddChunk(this, ChunkData.enemiesData);
    }

    public void Save()
    {
        ChunkData.prefabKeyData = prefabKey;
        ChunkData.isVisitedData = _isVisited;
        ChunkData.positionData = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        ChunkData.enemiesData = DS.GetSoManager<EnemiesManagerSo>().SaveEnemiesData(this);
    }

    public void EraseChunk(bool save)
    {
        _eventManager.onChunkDespawned?.Invoke(_intPosition, this, save);
    }

    private void OnDestroy()
    {
        _enemiesManager.ClearChunk(this);
    }
}

[Serializable]
public class ChunkData
{
    public string prefabKeyData;
    public bool isVisitedData;
    public float noiseData;
    public Vector2Int positionData;
    public List<EnemyData> enemiesData;
}
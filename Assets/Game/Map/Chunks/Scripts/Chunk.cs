using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Chunk : MonoBehaviour, ISaveAble
{
    [SerializeField] private string key;
    [SerializeField] private int cellSize;
    [SerializeField] private int minEnemies;
    [SerializeField] private int maxEnemies;
    [SerializeField] private List<Enemy> allowedEnemies;
    [SerializeField] private List<BoxCollider2D> spawnZones;
    public string InstanceKey { get => key; set => key = value; }
    public List<Enemy> AllowedEnemies => allowedEnemies;
    public ChunkData ChunkData { get; set; }
    public void Save()
    {
        ChunkData = new ChunkData
        {
            prefabKey = key,
            position = transform.position,
            enemies = DS.GetSoManager<EnemiesManagerSo>().SaveEnemies(this)
        };
    }

    public void Load()
    {
        transform.position = ChunkData.position;
        DS.GetSoManager<EnemiesManagerSo>().LoadEnemies(this);
    }

    public List<Enemy> GetRandomEnemies()
    {
        var enemiesCount = Random.Range(minEnemies, maxEnemies + 1);
        var randomEnemies = new List<Enemy>();
        for (var i = 0; i < enemiesCount; i++)
        {
            var enemy = allowedEnemies[Random.Range(0, allowedEnemies.Count - 1)];
            randomEnemies.Add(enemy);
        }
        return randomEnemies;
    }

    public Vector3 GetRandomSpawnPoint()
    {
        var zone = spawnZones[Random.Range(0, spawnZones.Count)];
        var bounds = zone.bounds;

        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);

        var xSnapped = Mathf.Round(x / cellSize) * cellSize;
        var ySnapped = Mathf.Round(y / cellSize) * cellSize;

        return new Vector3(xSnapped, ySnapped, 0f);
    }

    private void OnDestroy()
    {
        DS.GetSoManager<EnemiesManagerSo>().RemoveChunk(this);
    }
}

[Serializable]
public class ChunkData
{
    public string prefabKey;
    public Vector3 position;
    public List<EnemyData> enemies;
}

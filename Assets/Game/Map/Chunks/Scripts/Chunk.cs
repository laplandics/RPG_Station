using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    private CancellationTokenSource _cts;
    public string PrefabKey { get => key; set => key = value; } 
    public ChunkData ChunkData { get; set; } 
    public Task Save() 
    { 
        ChunkData = new ChunkData 
        { 
            prefabKey = key, 
            position = transform.position, 
        }; 
        return Task.CompletedTask;
    }
    
    public async Task Load() 
    { 
        while (ChunkData == null)
        {
            try { await Task.Yield(); }
            catch (TaskCanceledException) { return; }
        }
        transform.position = ChunkData!.position; 
    }

    public List<Enemy> GetRandonEnemies()
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
        var zone = spawnZones[Random.Range(0, spawnZones.Count - 1)];
        var bounds = zone.bounds;

        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);

        var xSnapped = Mathf.Round(x / cellSize) * cellSize;
        var ySnapped = Mathf.Round(y / cellSize) * cellSize;

        return new Vector3(xSnapped, ySnapped, 0f);
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
    }
}

[Serializable]
public class ChunkData
{
    public string prefabKey;
    public Vector3 position;
}

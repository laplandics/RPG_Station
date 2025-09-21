using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesManager", menuName = "ManagersSO/EnemiesManager")]
public class EnemiesManagerSo : ScriptableObject
{
    private Dictionary<Chunk, List<Enemy>> _currentEnemies = new();
    public void Initialize()
    {
        //DS.GetSoManager<EventManagerSo>().onChunkSpawned.AddListener(SpawnRandomEnemies);
        //DS.GetSoManager<EventManagerSo>().onChunkDespawned.AddListener(DespawnEnemies);
    }

    public void LoadEnemies(Chunk chunk)
    {
        DespawnEnemies(chunk);
        SpawnSavedEnemies(chunk);
    }

    public List<EnemyData> SaveEnemies(Chunk chunk)
    {
        var enemiesData = new List<EnemyData>();
        foreach (var enemy in _currentEnemies[chunk])
        {
            enemy.Save();
            enemiesData.Add(enemy.EnemyData);
        }

        return enemiesData;
    }

    private void DespawnEnemies(Chunk chunk)
    {
        if (!_currentEnemies.TryGetValue(chunk, out var chunkEnemies)) return;
        foreach (var enemy in chunkEnemies)
        {
            DS.GetSoManager<EventManagerSo>().onEnemyDespawned?.Invoke(enemy);
            Destroy(enemy.gameObject);
        }
        chunkEnemies.Clear();
        _currentEnemies[chunk].Clear();
    }

    private void SpawnRandomEnemies(Chunk chunk)
    {
        var chunkEnemies = new List<Enemy>();
        if (_currentEnemies.TryGetValue(chunk, out _)) return;
        foreach (var enemy in chunk.GetRandomEnemies())
        {
            var enemyInstance = Instantiate(enemy, chunk.GetRandomSpawnPoint(), Quaternion.identity, chunk.gameObject.transform);
            chunkEnemies.Add(InitializeEnemy(enemyInstance, chunk));
        }

        _currentEnemies.TryAdd(chunk, chunkEnemies);
    }

    private void SpawnSavedEnemies(Chunk chunk)
    {
        var chunkEnemies = new List<Enemy>();
        foreach (var enemyData in chunk.ChunkData.enemies)
        {
            foreach (var allowedEnemy in chunk.AllowedEnemies)
            {
                if (enemyData.prefabKey != allowedEnemy.InstanceKey) continue;
                var enemyInstance = Instantiate(allowedEnemy, enemyData.position, Quaternion.identity, chunk.gameObject.transform);
                chunkEnemies.Add(InitializeEnemy(enemyInstance, chunk));
                enemyInstance.EnemyData = enemyData;
                enemyInstance.Load();
            }
        }
        _currentEnemies.TryAdd(chunk, chunkEnemies);
    }

    public void RemoveChunk(Chunk chunk) => _currentEnemies.Remove(chunk);

    private Enemy InitializeEnemy(Enemy instance, Chunk chunk)
    {
        instance.Initialize();
        instance.gameObject.name = $"Enemy of {chunk.gameObject.name}";
        DS.GetSoManager<EventManagerSo>().onEnemySpawned?.Invoke(instance);

        return instance;
    }
}
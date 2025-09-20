using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "EnemiesManager", menuName = "ManagersSO/EnemiesManager")]
public class EnemiesManagerSo : ScriptableObject
{
    private Dictionary<Chunk, List<Enemy>> _currentEnemies = new();
    private Dictionary<Chunk, List<EnemyData>> _savedEnemies = new();
    public Task Initialize()
    {
        DS.GetSoManager<EventManagerSo>().onChunkSpawned.AddListener(SpawnRandomEnemies);
        DS.GetSoManager<EventManagerSo>().onChunkDespawned.AddListener(DespawnEnemies);
        return Task.CompletedTask;
    }

    public async Task LoadEnemies(Chunk chunk)
    {
        DespawnEnemies(chunk);
        await SpawnSavedEnemies(chunk);
    }

    public async Task<List<EnemyData>> SaveEnemies(Chunk chunk)
    {
        var enemiesData = new List<EnemyData>();
        foreach (var enemy in _currentEnemies[chunk])
        {
            await enemy.Save();
            enemiesData.Add(enemy.EnemyData);
        }

        return enemiesData;
    }

    private void DespawnEnemies(Chunk chunk)
    {
        if (!_currentEnemies.TryGetValue(chunk, out var chunkEnemies)) return;
        foreach (var enemy in chunkEnemies)
        {
            Destroy(enemy.gameObject);
        }
        _currentEnemies.Remove(chunk);
    }

    private void SpawnRandomEnemies(Chunk chunk)
    {
        var chunkEnemies = new List<Enemy>();
        if (_currentEnemies.TryGetValue(chunk, out _)) return;
        foreach (var enemy in chunk.GetRandonEnemies())
        {
            chunkEnemies.Add(Instantiate(enemy, chunk.GetRandomSpawnPoint(), Quaternion.identity));
        }

        _currentEnemies.TryAdd(chunk, chunkEnemies);
    }

    private async Task SpawnSavedEnemies(Chunk chunk)
    {
        var chunkEnemies = new List<Enemy>();
        foreach (var enemyData in chunk.ChunkData.enemies)
        {
            foreach (var allowedEnemy in chunk.AllowedEnemies)
            {
                if (enemyData.prefabKey != allowedEnemy.PrefabKey) continue;
                chunkEnemies.Add(Instantiate(allowedEnemy, enemyData.position, Quaternion.identity));
                chunkEnemies[^1].gameObject.name = $"Enemy of {chunk.gameObject.name}";
                await chunkEnemies[^1].Load();
            }
        }
        _currentEnemies.TryAdd(chunk, chunkEnemies);
    }
}
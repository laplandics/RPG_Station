using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "EnemiesManager", menuName = "ManagersSO/EnemiesManager")]
public class EnemiesManagerSo : ScriptableObject
{
    private Dictionary<Chunk, List<Enemy>> _currentEnemies = new();
    public Task Initialize()
    {
        DS.GetSoManager<EventManagerSo>().onChunkSpawned.AddListener(SpawnEnemies);
        DS.GetSoManager<EventManagerSo>().onChunkDespawned.AddListener(DespawnEnemies);
        return Task.CompletedTask;
    }

    private void DespawnEnemies(Chunk chunk)
    {
        foreach (var enemy in _currentEnemies[chunk])
        {
            Destroy(enemy.gameObject);
        }
        _currentEnemies.Remove(chunk);
    }

    private void SpawnEnemies(Chunk chunk)
    {
        var chunkEnemies = new List<Enemy>();
        foreach (var enemy in chunk.GetRandonEnemies())
        {
            chunkEnemies.Add(Instantiate(enemy, chunk.GetRandomSpawnPoint(), Quaternion.identity));
        }

        _currentEnemies.TryAdd(chunk, chunkEnemies);
    }
}
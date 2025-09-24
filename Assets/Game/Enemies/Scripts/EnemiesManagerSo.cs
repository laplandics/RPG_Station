using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesManager", menuName = "ManagersSO/EnemiesManager")]
public class EnemiesManagerSo : ScriptableObject
{
    private Dictionary<Chunk, List<Enemy>> _enemiesInScene = new();
    private Dictionary<Chunk, List<EnemyData>> _savedEnemies = new();
    private EnemiesSpawnManagerSo _enemiesSpawner;
    private EventManagerSo _eventManager;

    public void Initialize()
    {
        _enemiesSpawner = DS.GetSoSpawner<EnemiesSpawnManagerSo>();
        _eventManager = DS.GetSoManager<EventManagerSo>();

        _eventManager.onEnemySpawned.AddListener(AddEnemy);
        _eventManager.onEnemyDespawned.AddListener(RemoveEnemy);
    }

    public List<EnemyData> SaveEnemiesData(Chunk chunk)
    {
        _savedEnemies.TryAdd(chunk, new List<EnemyData>());
        foreach (var enemy in _enemiesInScene[chunk])
        {
            enemy.Save();
            _savedEnemies[chunk].Add(enemy.EnemyData);
        }

        return _savedEnemies[chunk];
    }

    private void AddEnemy(Chunk chunk, Enemy enemy) =>_enemiesInScene[chunk].Add(enemy);

    private void RemoveEnemy(Chunk chunk, Enemy enemy)
    {
        if (_enemiesInScene.ContainsKey(chunk)) _enemiesInScene[chunk].Remove(enemy);
        if (_enemiesInScene[chunk].Count == 0) _enemiesInScene.Remove(chunk);
    }

    public void ClearChunk(Chunk chunk)
    {
        for (var i = _enemiesInScene[chunk].Count - 1; i >= 0; i--)
        {
            _enemiesSpawner.DespawnEnemy(chunk, _enemiesInScene[chunk][i]);
        }
    }

    public void AddChunk(Chunk chunk, List<EnemyData> enemiesList)
    {
        _enemiesInScene.Add(chunk, new List<Enemy>());
        if (enemiesList == null) _enemiesSpawner.SpawnRandomEnemies(chunk);
        else _enemiesSpawner.SpawnSavedEnemies(chunk, enemiesList);
    }
}
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "EnemiesSpawner", menuName = "SpawnersSO/EnemiesSpawner")]
public class EnemiesSpawnManagerSo : ScriptableObject, ISpawner
{
    private EnemiesManagerSo _enemiesManager;
    private EventManagerSo _eventManager;
    public void InitializeSpawner()
    {
        _enemiesManager = DS.GetSoManager<EnemiesManagerSo>();
        _eventManager = DS.GetSoManager<EventManagerSo>();
    }

    public void SpawnRandomEnemies(Chunk chunk)
    {
        var enemies = GetRandomenemies(chunk);
        foreach (var enemy in enemies)
        {
            var enemyInstance = Instantiate(enemy, GetRandomSpawnPoint(chunk), Quaternion.identity, chunk.gameObject.transform);
            InitializeEnemy(chunk, new EnemyData(), enemyInstance);
        }
    }

    public void SpawnSavedEnemies(Chunk chunk, List<EnemyData> data)
    {
        foreach (var enemyData in data)
        {
            foreach (var enemyPrefab in chunk.AllowedEnemies)
            {
                if (enemyData.prefabKeyData != enemyPrefab.InstanceKey) continue;
                var enemyInstance = Instantiate(enemyPrefab, enemyData.positionData, Quaternion.identity, chunk.gameObject.transform);
                InitializeEnemy(chunk, enemyData, enemyInstance);
            }
        }
    }

    public void DespawnEnemy(Chunk chunk, Enemy enemy)
    {
        Destroy(enemy.gameObject);
        _eventManager.onEnemyDespawned?.Invoke(chunk, enemy);
    }

    private List<Enemy> GetRandomenemies(Chunk chunk)
    {
        var enemiseCount = Random.Range(chunk.MinEnemies, chunk.MaxEnemies + 1);
        var enemies = new List<Enemy>();
        for (var i = 0; i < enemiseCount; i++)
        {
            var enemy = chunk.AllowedEnemies[Random.Range(0, chunk.AllowedEnemies.Length)];
            enemies.Add(enemy);
        }

        return enemies;
    }

    private Vector2 GetRandomSpawnPoint(Chunk chunk)
    {
        var zone = chunk.SpawnZones[Random.Range(0, chunk.SpawnZones.Length)];
        var bounds = zone.bounds;

        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);

        var xSnapped = GridMover.SnapToGrid(new Vector2(x, y));

        return xSnapped;
    }

    private void InitializeEnemy(Chunk chunk, EnemyData data, Enemy enemy)
    {
        enemy.gameObject.name = $"Enemy of {chunk.gameObject.name}";
        enemy.Initialize(chunk);
        enemy.EnemyData = data;
        enemy.Load();

        _eventManager.onEnemySpawned?.Invoke(chunk, enemy);
    }
}

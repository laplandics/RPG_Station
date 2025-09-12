using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesPersistentData : MonoBehaviour, IPersistentData
{
    public void Save(ref SaveloadSystem.SaveData data)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        data.enemiesData = new EnemiesSaveData();
        
        foreach (var enemy in enemies)
        {
            data.enemiesData.enemiesState.Add(new EnemyData {id = enemy.Id, state = enemy.State});
        }
    }

    public void Load(SaveloadSystem.SaveData data)
    { 
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (var enemyData in data.enemiesData.enemiesState)
        {
            foreach (var enemy in enemies)
            {
                if (enemyData.id != enemy.Id) continue;
                enemy.SetState(enemyData.state);
            }
        }
    }
}

[Serializable]
public class EnemiesSaveData
{
    public List<EnemyData> enemiesState = new();
    
}

[Serializable]
public class EnemyData
{
    public string id;
    public bool state;
}
using UnityEngine;

public class EnemiesPersistentData : MonoBehaviour, IPersistentData
{
    public void Save(SaveDataSO data)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (var enemy in enemies)
        {
            data.saveData.enemiesSaveData.enemiesStates.Add(new EnemiesSaveData.EnemyData {id = enemy.Id, state = enemy.State});
        }
    }

    public void Load(SaveDataSO data)
    { 
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (var enemyData in data.saveData.enemiesSaveData.enemiesStates)
        {
            foreach (var enemy in enemies)
            {
                if (enemyData.id != enemy.Id) continue;
                enemy.SetState(enemyData.state);
            }
        }
    }
}
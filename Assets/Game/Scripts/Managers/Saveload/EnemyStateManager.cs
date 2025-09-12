using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public static EnemyStateManager Instance  { get; private set; }
    private Dictionary<string, bool> _enemiesIdState = new();
    
    private void Awake()
    {
        if (!Instance) {Instance = this; DontDestroyOnLoad(gameObject);}
        else Destroy(gameObject);
    }

    public void SetEnemiesIdDictionary(string id)
    {
        if (!_enemiesIdState.TryAdd(id, false)) {}
    }
    
    public Dictionary<string, bool> GetEnemiesIdDictionary() => _enemiesIdState;

    public void SetNewEnemyStateById(string id, bool isDead)
    {
        _enemiesIdState[id] = isDead;
    }

    public void SaveNewData()
    {
        var gameData = SaveloadSystem.GetSaveDataFromFile();
        
        foreach (var enemyData in gameData.enemiesData.enemiesState)
        {
            if (!_enemiesIdState.TryGetValue(enemyData.id, out var state)) continue;
            enemyData.state = state;
        }
        
        _enemiesIdState.Clear();
        SaveloadSystem.SetNewDataToFile(gameData);
    }
}

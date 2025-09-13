using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoadSystemManager", menuName = "ManagersSO/SaveLoadSystemManager")]

public class SaveLoadSystemManagerSO : ScriptableObject
{
    [Serializable]
    public struct SaveData
    {
        public EnemiesSaveData enemies;
        public PlayerSaveData player;
    }
    private SaveData saveData;

    public void Save()
    {
        saveData.enemies = SaveEnemies();
        saveData.player = SavePlayer();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
    }

    private string SaveFileName()
    {
        return Path.Combine(Application.persistentDataPath, "save.json");
    }
    
    private PlayerSaveData SavePlayer()
    {
        var player = FindFirstObjectByType<Player>();
        return new PlayerSaveData {position = player.transform.position, sprite = player.SpriteRenderer.sprite};
    }

    private EnemiesSaveData SaveEnemies()
    {
        var enemySaveData = new EnemiesSaveData();
        
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            enemySaveData.enemiesData.Add(new EnemiesSaveData.EnemyData {id = enemy.Id, position = enemy.transform.position, isDead = enemy.State});
        }

        return enemySaveData;
    }

    public void Load(out EnemiesSaveData enemiesData, out PlayerSaveData playerData)
    {
        if (!File.Exists(SaveFileName()))
        {
            enemiesData = new EnemiesSaveData();
            playerData = new PlayerSaveData();
            return;
        }
        
        saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(SaveFileName()));
        enemiesData = saveData.enemies;
        playerData = saveData.player;
    }
}
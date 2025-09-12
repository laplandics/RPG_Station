using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public class SaveloadSystem
{
    [Serializable]
    public struct SaveData
    {
        public PlayerSaveData playerData;
        public EnemiesSaveData enemiesData;
    }
    
    private static SaveData _saveData;
    
    private static List<IPersistentData> _persistentDataStorages;

    private static void FindPersistentDataStorages()
    {
        _persistentDataStorages = new List<IPersistentData>();
        var objects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None); 
        foreach (var obj in objects)
        {
            if (obj is IPersistentData persistent)
            {
                _persistentDataStorages.Add(persistent);
            }
        }
    }

    private static string SaveFileName()
    {
        return Path.Combine(Application.persistentDataPath, "save.json");
    }

    public static void SaveAll()
    {
        FindPersistentDataStorages();

        foreach (var storage in _persistentDataStorages)
        {
            storage.Save(ref _saveData);
        }

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
        GameManager.Instance.OnAllPersistentDataSaved?.Invoke();
    }

    public static void LoadAll()
    {
        if (!File.Exists(SaveFileName())) {Debug.LogWarning("Save file not found!"); return;}

        _saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(SaveFileName()));
        FindPersistentDataStorages();

        foreach (var persistent in _persistentDataStorages)
        {
            persistent.Load(_saveData);
        }
        GameManager.Instance.OnAllPersistentDataLoaded?.Invoke();
    }

    public static SaveData GetSaveDataFromFile()
    {
        _saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(SaveFileName()));
        return _saveData;
    }


    public static void SetNewDataToFile(SaveData saveData)
    {
        _saveData = saveData;
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }
}
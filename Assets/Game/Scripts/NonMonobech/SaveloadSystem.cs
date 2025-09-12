using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public class SaveloadSystem
{
    public static SaveDataSO SaveDataSo
    {
        set
        {
            _saveDataSo = value;
            _saveData = value.saveData;
        }
    }
    private static SaveDataSO _saveDataSo;
    private static SaveDataSO.SaveData _saveData;
    private static List<IPersistentData> _persistentDataStorages;

    private static void FindPersistentDataObjects()
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
        if (!_saveDataSo) Debug.LogError("SaveDataSo is null");
        if (_saveDataSo.saveData.playerSaveData == null) Debug.LogError("SaveDataSo.saveData.playerSaveData is null");
        
        
        FindPersistentDataObjects();
        foreach (var storage in _persistentDataStorages)
        {
            storage.Save(_saveDataSo);
        }
        _saveData = _saveDataSo.saveData;
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
        GameManager.Instance.OnAllPersistentDataSaved?.Invoke();
    }

    public static void LoadAll()
    {
        if (!File.Exists(SaveFileName())) return;

        _saveData = JsonUtility.FromJson<SaveDataSO.SaveData>(File.ReadAllText(SaveFileName()));
        _saveDataSo.saveData = _saveData;
        FindPersistentDataObjects();

        foreach (var persistent in _persistentDataStorages)
        {
            persistent.Load(_saveDataSo);
        }
        GameManager.Instance.OnAllPersistentDataLoaded?.Invoke();
    }

    public static SaveDataSO GetSaveDataFromFile()
    {
        _saveData = JsonUtility.FromJson<SaveDataSO.SaveData>(File.ReadAllText(SaveFileName()));
        _saveDataSo.saveData = _saveData;
        return _saveDataSo;
    }


    public static void SetNewDataToFile(SaveDataSO saveDataSo)
    {
        _saveData = saveDataSo.saveData;
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }
}
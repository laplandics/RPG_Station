using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoadSystemManager", menuName = "ManagersSO/SaveLoadSystemManager")]
public class SaveLoadManagerSo : ScriptableObject, IInSceneManagerListener
{
    private bool _isRoutineManagerAvailable;
    public void OnSceneManagersInitialized() => _isRoutineManagerAvailable = true;
    
    public void Save(string key, object data)
    {
        
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        if (!_isRoutineManagerAvailable) {Debug.LogError("Error during saving gameData: RoutineManager is unavailable"); return;}
        DS.GetSceneManager<RoutineManager>().StartRoutine(SerializeSaveFileRoutine(key, json, _ => {}));
    }

    private IEnumerator SerializeSaveFileRoutine(string key, string json, Action<bool> callback)
    {
        var task = File.WriteAllTextAsync(GetFile(key), json);
        while (!task.IsCompleted) yield return null;
        if (task.IsFaulted) {Debug.LogError("Error during saving gameData: " + task.Exception); callback?.Invoke(false);}
        else callback?.Invoke(true);
    }
    
    public void Load<T>(string key, Action<T> callback)
    {
        if (!_isRoutineManagerAvailable) {Debug.LogError("Error during loading gameData: RoutineManager is unavailable"); return;}
        DS.GetSceneManager<RoutineManager>().StartRoutine(DeserializeSaveFileRoutine(key, (callbackResult, taskResult) =>
        {
            if (!callbackResult) return;
            var data = JsonConvert.DeserializeObject<T>(taskResult);
            callback?.Invoke(data);
        }));
    }

    private IEnumerator DeserializeSaveFileRoutine(string key, Action<bool, string> callback)
    {
        var task = File.ReadAllTextAsync(GetFile(key));
        while (!task.IsCompleted) yield return null;
        if (task.IsFaulted) {Debug.LogError("Error during loading gameData: " + task.Exception); callback?.Invoke(false, null);}
        else callback?.Invoke(true, task.Result);
    }
    
    private string GetFile(string key) => Path.Combine(Application.persistentDataPath, $"{key}.json");
}
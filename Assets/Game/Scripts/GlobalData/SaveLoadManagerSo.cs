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
        var json = JsonConvert.SerializeObject(data, GetSettings());
        if (!_isRoutineManagerAvailable) {Debug.LogError("Error during saving gameData: RoutineManager is unavailable"); return;}
        File.WriteAllText(GetFile(key), json);
    }
    
    public T Load<T>(string key)
    {
        if (!_isRoutineManagerAvailable) { Debug.LogError("Error during loading gameData: RoutineManager is unavailable"); return default; }
        var task = File.ReadAllText(GetFile(key));
        var data = JsonConvert.DeserializeObject<T>(task, GetSettings());

        return data;
    }

    private JsonSerializerSettings GetSettings()
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };

        return settings;
    }

    private string GetFile(string key) => Path.Combine(Application.persistentDataPath, $"{key}.json");
}
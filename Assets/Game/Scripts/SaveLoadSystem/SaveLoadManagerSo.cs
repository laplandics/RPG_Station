using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoadSystemManager", menuName = "ManagersSO/SaveLoadSystemManager")]
public class SaveLoadManagerSo : ScriptableObject, IInSceneManagerListener
{
    public void OnSceneManagersInitialized() {}
    
    public void Save(string key, object data)
    {
        var json = JsonConvert.SerializeObject(data, GetSettings());
        File.WriteAllText(GetFile(key), json);
    }
    
    public T Load<T>(string key)
    {
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
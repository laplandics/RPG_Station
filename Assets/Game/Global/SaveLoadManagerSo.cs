using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoadSystemManager", menuName = "ManagersSO/SaveLoadSystemManager")]
public class SaveLoadManagerSo : ScriptableObject, IInSceneManagerListener
{
    private bool _isRoutineManagerAvailable;
    public void OnSceneManagersInitialized() => _isRoutineManagerAvailable = true;
    
    public async Task Save(string key, object data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        if (!_isRoutineManagerAvailable) {Debug.LogError("Error during saving gameData: RoutineManager is unavailable"); return;}
        await File.WriteAllTextAsync(GetFile(key), json);
    }
    
    public async Task<T> Load<T>(string key)
    {
        if (!_isRoutineManagerAvailable) return default;
        var task = await File.ReadAllTextAsync(GetFile(key));
        var data = JsonConvert.DeserializeObject<T>(task);

        return data;
    }

    
    private string GetFile(string key) => Path.Combine(Application.persistentDataPath, $"{key}.json");
}
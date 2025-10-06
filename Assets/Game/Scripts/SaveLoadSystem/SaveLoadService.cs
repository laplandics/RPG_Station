using UnityEngine;
using static EventManager;

public static class SaveLoadService
{
    private static SceneInitializeService _sceneInitializeService;
    
    public static void SubscribeToSaveLoadEvents()
    {
        _sceneInitializeService = DS.GetSceneManager<SceneInitializeService>();
        OnLoad.AddListener(LoadGame);
        OnSave.AddListener(SaveGame);
    }

    private static void LoadGame()
    {
        DS.GetGlobalManager<GlobalInputsManagerSo>().DisableAllInputs();
        _sceneInitializeService.SetDataState(false);
        _sceneInitializeService.EraseScene();
        var handlers = DS.GetGlobalManager<GlobalDataManagerSo>().GetAllHandlers();
        foreach (var handler in handlers.Values) { if (!handler.TryLoadData()) Debug.LogError($"{handler.GetType()} failed to load game data"); }
        _sceneInitializeService.SetDataState(true);
        Debug.LogWarning("Game data loaded");
    }

    private static void SaveGame()
    {
        DS.GetGlobalManager<GlobalInputsManagerSo>().DisableAllInputs();
        var handlers = DS.GetGlobalManager<GlobalDataManagerSo>().GetAllHandlers();
        foreach (var handler in handlers.Values) { handler.TrySaveData(); }
        DS.GetGlobalManager<GlobalInputsManagerSo>().EnableAllInputs();
        Debug.LogWarning("Save Game Complete");
    }
}
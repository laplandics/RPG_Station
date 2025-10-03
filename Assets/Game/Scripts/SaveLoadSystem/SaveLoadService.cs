using UnityEngine;
using static EventManager;

public static class SaveLoadService
{
    private static SceneInitializeService _sceneInitializeService;
    private static SaveLoadManagerSo _saveLoadManager;
    
    public static void SubscribeToSaveLoadEvents()
    {
        _sceneInitializeService = DS.GetSceneManager<SceneInitializeService>();
        _saveLoadManager = DS.GetGlobalManager<SaveLoadManagerSo>();
        OnLoad.AddListener(LoadGame);
        OnSave.AddListener(SaveGame);
    }

    private static void LoadGame()
    {
        DS.GetGlobalManager<GlobalInputsManagerSo>().DisableAllInputs();
        _sceneInitializeService.SetDataState(false);
        _sceneInitializeService.EraseScene();
        
        var mapData = _saveLoadManager.Load<MapData>(MapDataHandler.GetMapData.instanceKey);
        var playerData = _saveLoadManager.Load<PlayerData>(PlayerDataHandler.GetPlayerData.instanceKey);
        var terrainsData = _saveLoadManager.Load<AllTerrainsData>(TerrainDataHandler.GetAllTerrainsData.instanceKey);
        
        MapDataHandler.SetMapData(mapData);
        PlayerDataHandler.SetPlayerData(playerData);
        TerrainDataHandler.SetTerrainsData(terrainsData);
        
        _sceneInitializeService.SetDataState(true);
    }

    private static void SaveGame()
    {
        DS.GetGlobalManager<GlobalInputsManagerSo>().DisableAllInputs();
        var mapData = MapDataHandler.SetMapData();
        var playerData = PlayerDataHandler.SetPlayerData();
        var terrainsData = TerrainDataHandler.SetTerrainsData();
        
        _saveLoadManager.Save(mapData.instanceKey, mapData);
        _saveLoadManager.Save(playerData.instanceKey, playerData);
        _saveLoadManager.Save(terrainsData.instanceKey, terrainsData);
        
        DS.GetGlobalManager<GlobalInputsManagerSo>().EnableAllInputs();
        Debug.LogWarning("Save Game Complete");
    }
}
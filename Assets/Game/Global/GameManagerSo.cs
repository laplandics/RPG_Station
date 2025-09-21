using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;


[CreateAssetMenu(fileName = "GameManager", menuName = "ManagersSO/GameManager")]
public class GameManagerSo : ScriptableObject
{
    [SerializeField] private MainGameObjectsSo instances;
    private Camera _camera;
    private Light2D _globalLight;
    private MapManagerSo _mapManager;
    private ChunksManagerSo _chunksManager;
    private PlayerManagerSo _playerManager;
    private EnemiesManagerSo _enemiesManager;
    private UIManagerSo _uiManager;

    public async Task Initialize()
    {
        _playerManager = DS.GetSoManager<PlayerManagerSo>();
        _mapManager = DS.GetSoManager<MapManagerSo>();
        _chunksManager = DS.GetSoManager<ChunksManagerSo>();
        _enemiesManager = DS.GetSoManager<EnemiesManagerSo>();
        _uiManager = DS.GetSoManager<UIManagerSo>();

        await AssignEvents();
        await InitializeObjectsManagers();
    }

    private Task AssignEvents()
    {
        var eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onUnityEssentialsSpawned.AddListener(AssignUnityEssentials);
        eventManager.onSave.AddListener(() => _ = TrySaveGame());
        eventManager.onLoad.AddListener(() => _ = TryLoadGame());
        return Task.CompletedTask;
    }

    private void AssignUnityEssentials(Light2D light, Camera camera)
    {
        _globalLight = light;
        _camera = camera;
    }

    private async Task InitializeObjectsManagers()
    {
        await _uiManager.Initialize();
        await _playerManager.Initialize();
        await _mapManager.Initialize();
        await _chunksManager.Initialize();
        await _enemiesManager.Initialize();
    }
    
    private async Task TryLoadGame()
    {
        await _playerManager.LoadPlayerData();
        await _mapManager.LoadMapData();
    }

    private async Task TrySaveGame()
    { 
        if (_playerManager) await _playerManager.SavePlayerData();
        if (_chunksManager) await _chunksManager.SaveChunksData();
        if (_mapManager) await _mapManager.SaveMapData();
    }
}

using System;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "GameManager", menuName = "ManagersSO/GameManager")]
public class GameManagerSo : ScriptableObject
{
    [SerializeField] private InitInstances instances;
    private GameObject _cameraInstance;
    private GameObject _globalLightningInstance;
    private Map _map;
    private MapManagerSo _mapManager;
    private ChunksManagerSo _chunksManager;
    private Player  _player;
    private PlayerManagerSo _playerManager;
    private bool _isInitialized;

    public async Task Initialize(GameObject cameraInstance, GameObject globalLightningInstance)
    {
        _cameraInstance = cameraInstance;
        _globalLightningInstance = globalLightningInstance;
        _playerManager = DS.GetSoManager<PlayerManagerSo>();
        _mapManager = DS.GetSoManager<MapManagerSo>();
        _chunksManager = DS.GetSoManager<ChunksManagerSo>();

        await AssignEvents();

        await InstantiateSceneObjects(() => _isInitialized);
        InitializeCameraFollow();
    }

    private Task AssignEvents()
    {
        var eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onMapUpdated.AddListener(EndInitialization);
        eventManager.onSave.AddListener(() => _ = TrySaveGame());
        eventManager.onLoad.AddListener(() => _ = TryLoadGame());
        return Task.CompletedTask;
    }

    private async Task InstantiateSceneObjects(Func<bool> isInitialized)
    {
        _player = await _playerManager.SpawnPlayer(instances.playerPrefab);
        _map = await _mapManager.SpawnMap();
        await _chunksManager.SpawnChunks(_map.transform);
        while (!isInitialized()) {await Task.Yield();}
    }
    
    private void EndInitialization() => _isInitialized = true;
    

    private void InitializeCameraFollow()
    {
        var cameraFollow = _cameraInstance.GetComponent<GameCamera>();
        cameraFollow.SetTarget(_player.transform);
    }

    private async Task TryLoadGame()
    {
        await _playerManager.LoadPlayerData();
        await _mapManager.LoadMapData();
    }

    private async Task TrySaveGame()
    { 
        if (_player) await _playerManager.SavePlayerData();
        if (_chunksManager) await _chunksManager.SaveChunksData();
        if (_mapManager) await _mapManager.SaveMapData();
    }
}

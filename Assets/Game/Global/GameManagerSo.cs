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
    private ChunkManagerSo _chunkManager;
    private Player  _player;
    private PlayerManagerSo _playerManager;
    private bool _isInitialized;

    public async Task Initialize(GameObject cameraInstance, GameObject globalLightningInstance)
    {
        _cameraInstance = cameraInstance;
        _globalLightningInstance = globalLightningInstance;
        _playerManager = DS.GetSoManager<PlayerManagerSo>();
        _mapManager = DS.GetSoManager<MapManagerSo>();
        _chunkManager = DS.GetSoManager<ChunkManagerSo>();
        
        DS.GetSoManager<EventManagerSo>().onMapUpdated.AddListener(EndInitialization);
        DS.GetSoManager<EventManagerSo>().onSave.AddListener(TrySaveGame);
        DS.GetSoManager<EventManagerSo>().onLoad.AddListener(() => _ = TryLoadGame());

        await InstantiateSceneObjects(() => _isInitialized);
        InitializeCameraFollow();
    }

    private async Task InstantiateSceneObjects(Func<bool> isInitialized)
    {
        _player = await _playerManager.SpawnPlayer(instances.playerPrefab);
        _map = await _mapManager.SpawnMap();
        await _chunkManager.SpawnChunks(_map.transform);
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
    }

    private void TrySaveGame()
    { 
        if (!_player) return;
        _playerManager.SavePlayerData();
    }
}

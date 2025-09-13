using UnityEngine;


[CreateAssetMenu(fileName = "GameManager", menuName = "ManagersSO/GameManager")]
public class GameManagerSo : ScriptableObject
{
    public bool isStartingNewGame;
    [SerializeField] private InitInstances instances;
    private GameObject _cameraInstance;
    private GameObject _globalLightningInstance;
    private GameObject _map;
    private Player  _player;
    private PlayerManagerSo _playerManager;
    private MapManagerSo _mapManager;
    
    public void Initialize(GameObject cameraInstance, GameObject globalLightningInstance)
    {
        _cameraInstance = cameraInstance;
        _globalLightningInstance = globalLightningInstance;
        
        InstantiateSceneObjects();
        InitializeCameraFollow();
        
        AssignLocalManagersToSaveEvent();
        AssignLocalManagersToLoadEvent();
        
        if (!isStartingNewGame) LoadGame();
    }

    private void AssignLocalManagersToSaveEvent()
    {
        var eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onSave.AddListener(_playerManager.SavePlayerData);
    }

    private void AssignLocalManagersToLoadEvent()
    {
        var eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onLoad.AddListener(_playerManager.LoadPlayerData);
    }

    private void InstantiateSceneObjects()
    {
        _playerManager = DS.GetSoManager<PlayerManagerSo>();
        _mapManager = DS.GetSoManager<MapManagerSo>();
        _player = _playerManager.SpawnPlayer(instances.playerPrefab);
        _map = _mapManager.SpawnMap(instances.mapPrefab);
    }

    private void InitializeCameraFollow()
    {
        var cameraFollow = _cameraInstance.GetComponent<CameraFollow>();
        cameraFollow.SetTarget(_player.transform);
    }

    private void LoadGame()
    {
        DS.GetSoManager<PlayerManagerSo>().LoadPlayerData();
    }
}

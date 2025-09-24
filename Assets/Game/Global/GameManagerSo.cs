using UnityEngine;
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

    public void Initialize()
    {
        _playerManager = DS.GetSoManager<PlayerManagerSo>();
        _mapManager = DS.GetSoManager<MapManagerSo>();
        _chunksManager = DS.GetSoManager<ChunksManagerSo>();
        _enemiesManager = DS.GetSoManager<EnemiesManagerSo>();
        _uiManager = DS.GetSoManager<UIManagerSo>();

        AssignEvents();
        InitializeObjectsManagers();
    }

    private void AssignEvents()
    {
        var eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onUnityEssentialsSpawned.AddListener(AssignUnityEssentials);
        eventManager.onSave.AddListener(TrySaveGame);
        eventManager.onLoad.AddListener(TryLoadGame);
    }

    private void AssignUnityEssentials(Light2D light, Camera camera)
    {
        _globalLight = light;
        _camera = camera;
    }

    private void InitializeObjectsManagers()
    {
        _uiManager.Initialize();
        _playerManager.Initialize();
        _mapManager.Initialize();
        _chunksManager.Initialize();
        _enemiesManager.Initialize();
    }
    
    private void TryLoadGame()
    {
        _playerManager.LoadPlayerData();
        _mapManager.LoadMapData();
    }

    private void TrySaveGame()
    { 
        if (_playerManager) _playerManager.SavePlayerData();
        if (_mapManager) _mapManager.SaveMapData();
    }
}

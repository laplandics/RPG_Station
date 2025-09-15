using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "GameManager", menuName = "ManagersSO/GameManager")]
public class GameManagerSo : ScriptableObject
{
    [SerializeField] private InitInstances instances;
    private GameObject _cameraInstance;
    private GameObject _globalLightningInstance;
    private Player  _player;
    private PlayerManagerSo _playerManager;

    public IEnumerator Initialize(GameObject cameraInstance, GameObject globalLightningInstance)
    {
        _cameraInstance = cameraInstance;
        _globalLightningInstance = globalLightningInstance;
        var routineManager = DS.GetSceneManager<RoutineManager>();
        
        DS.GetSoManager<EventManagerSo>().onSave.AddListener(() => routineManager.StartRoutine(TrySaveGame()));
        DS.GetSoManager<EventManagerSo>().onLoad.AddListener(() => routineManager.StartRoutine(TryLoadGame()));
        
        yield return routineManager.StartRoutine(InstantiateSceneObjects());
        yield return routineManager.StartRoutine(InitializeCameraFollow());
    }

    private IEnumerator InstantiateSceneObjects()
    {
        _playerManager = DS.GetSoManager<PlayerManagerSo>();
        _player = _playerManager.SpawnPlayer(instances.playerPrefab);
        yield return null;
    }

    private IEnumerator InitializeCameraFollow()
    {
        var cameraFollow = _cameraInstance.GetComponent<GameCamera>();
        cameraFollow.SetTarget(_player.transform);
        yield return null;
    }

    private IEnumerator TryLoadGame()
    {
        _playerManager.LoadPlayerData();
        yield return null;
    }

    private IEnumerator TrySaveGame()
    {
        _playerManager.SavePlayerData();
        yield return null;
    }
}

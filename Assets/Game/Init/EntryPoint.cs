using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class EntryPoint : MonoBehaviour
{
    public UnityEvent onInSceneManagersInitialized;
    
    [SerializeField] private InitPrefabsSO initPrefabs;
    [SerializeField] private InitialSettingsSO initialSettings;
    [SerializeField] private GameObject[] inSceneManagers;
    private GameManagerSO gameManager;
    
    private IEnumerator Start()
    {
        DS.Initialize();
        DS.GetManager<SaveLoadSystemManagerSO>().Load(out var enemiesData, out var playerData);

        AddUnityEventsListeners();
        
        SpawnInSceneManagers();
        SpawnGlobalLight();
        var playerPos = SpawnPlayer(playerData);
        SpawnCamera(playerPos);
        SpawnMap();
        SpawnSavedEnemies(enemiesData);
        
        yield break;
    }

    private void AddUnityEventsListeners()
    {
        foreach (var manager in  DS.GetAllManagers())
        {
            if (manager is not IRoutineManagerSo rManager) continue;
            onInSceneManagersInitialized.AddListener(rManager.OnRoutineAvailable);
        }
    }

    private void SpawnInSceneManagers()
    {
        foreach (var managerObject in inSceneManagers)
        {
            var manager = Instantiate(managerObject, Vector3.zero, Quaternion.identity).GetComponent<MonoBehaviour>();
            DS.SetManager(manager);
            manager.GetComponent<IInitializable>().Initialize();
        }
        onInSceneManagersInitialized?.Invoke();
    }

    private void SpawnGlobalLight()
    {
        Instantiate(initPrefabs.globalLightning, initPrefabs.globalLightning.transform.position, Quaternion.identity);
    }

    private Transform SpawnPlayer(PlayerSaveData playerData)
    {
        var player = Instantiate(initPrefabs.player, playerData.position, Quaternion.identity).GetComponent<Player>();
        if (playerData.sprite) player.SpriteRenderer.sprite = playerData.sprite;
        return player.transform;
    }
    
    private void SpawnCamera(Transform followTarget)
    {
        var cameraInstance = Instantiate(initPrefabs.camera, initPrefabs.camera.transform.position, Quaternion.identity);
        var cameraCm = cameraInstance.GetComponentInChildren<CinemachineCamera>();
        cameraCm.Follow = followTarget;
    }

    private void SpawnMap()
    {
        Instantiate(initPrefabs.map, initPrefabs.map.transform.position, Quaternion.identity);
    }

    private void SpawnSavedEnemies(EnemiesSaveData enemiesData)
    {
        foreach (var enemy in initialSettings.Enemies)
        {
            foreach (var enemyData in enemiesData.enemiesData)
            {
                if (enemyData.id != enemy.enemyPrefab.GetComponent<Enemy>().Id) continue;
                for (var i = 0; i < enemy.amount; i++)
                {
                    var someEnemy = Instantiate(enemy.enemyPrefab, enemyData.position,  Quaternion.identity).GetComponent<Enemy>();
                    someEnemy.SetState(enemyData.isDead);
                }
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private InitEssential initPrefabs;
    [SerializeField] private GameObject[] inSceneManagers;
    private GameObject _cameraInstance;
    private GameObject _globalLightningInstance;

    private async void Start()
    {
        try
        {
            await InitializeDS();
            await AssignManagersSoToInSceneManagersInitialization();
            await SpawnInSceneManagers();
            await SpawnUnityEssentials();
            DS.GetSoManager<GlobalInputsManagerSo>().EnableAllGlobalInputs();
            await InitializeGameManager();
    
            Debug.LogWarning("TODO: New Scene Load System");
            await CompleteInitialization();
        }
        catch (Exception e)
        {
            Debug.LogError($"An error occured while initializing the scene: {e.Message}");
        }
    }

    private static Task InitializeDS()
    {
        DS.Initialize();
        DS.GetSoManager<GlobalInputsManagerSo>().DisableAllGlobalInputs();
        return Task.CompletedTask;
    }

    private static async Task AssignManagersSoToInSceneManagersInitialization()
    {
        foreach (var manager in DS.GetSoManagers())
        {
            if (manager is not IInSceneManagerListener rManager) continue;
            DS.GetSoManager<EventManagerSo>().onInSceneManagersInitialized.AddListener(rManager.OnSceneManagersInitialized);
            await Task.Yield();
        }
    }

    private async Task SpawnInSceneManagers()
    {
        foreach (var managerObject in inSceneManagers)
        {
            var manager = Instantiate(managerObject, Vector3.zero, Quaternion.identity);
            DS.SetSceneManager(manager.GetComponent<MonoBehaviour>());
            manager.GetComponent<IInSceneManager>().Initialize();
            await Task.Yield();
        }
        DS.GetSoManager<EventManagerSo>().onInSceneManagersInitialized?.Invoke();
    }

    private Task SpawnUnityEssentials()
    {
        _globalLightningInstance = Instantiate(initPrefabs.globalLightning, initPrefabs.globalLightning.transform.position, Quaternion.identity);
        _cameraInstance = Instantiate(initPrefabs.camera, initPrefabs.camera.transform.position, Quaternion.identity);
        return Task.CompletedTask;
    }

    private async Task InitializeGameManager()
    {
        await DS.GetSoManager<GameManagerSo>().Initialize(_cameraInstance, _globalLightningInstance);
    }

    private static Task CompleteInitialization()
    {
        DS.GetSoManager<EventManagerSo>().onSceneInitializationCompleted?.Invoke();
        return Task.CompletedTask;
    }
}

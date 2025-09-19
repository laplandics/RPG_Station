using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] inSceneManagers;

    private async void Start()
    {
        try
        {
            await InitializeDS();
            await AssignManagersSoToInSceneManagersInitialization();
            await SpawnInSceneManagers();
            DS.GetSoManager<GlobalInputsManagerSo>().EnableAllGlobalInputs();
            await InitializeGameManager();
            
            DS.GetSoManager<EventManagerSo>().onManagersInitialized.Invoke();
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

    private async Task InitializeGameManager()
    {
        await DS.GetSoManager<GameManagerSo>().Initialize();
    }
}

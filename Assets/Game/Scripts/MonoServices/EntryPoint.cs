using UnityEngine;
using static EventManager;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private SceneInitializeService sceneInitializer;
    [SerializeField] private GameObject[] sceneServices;
    
    [SerializeField] private GlobalSettings globalSettings;
    private void Start()
    {
        InitializeDS();
        OnSceneReady.AddListener(FinalizeLoading);

        DS.GetGlobalManager<GlobalInputsManagerSo>().DisableAllGlobalInputs();
        AssignManagersSoToInSceneManagersInitialization();
        SpawnInSceneManagers();
        DS.GetGlobalManager<GlobalInputsManagerSo>().EnableAllGlobalInputs();
        InitializeGameSettings();
        InitializeSceneService();
    }

    private void InitializeDS()
    {
        DS.Initialize();
    }

    private void AssignManagersSoToInSceneManagersInitialization()
    {
        foreach (var manager in DS.GetGlobalManagers())
        {
            if (manager is not IInSceneManagerListener rManager) continue;
            OnInSceneManagersInitialized.AddListener(rManager.OnSceneManagersInitialized);
        }
    }

    private void SpawnInSceneManagers()
    {
        foreach (var service in sceneServices)
        {
            var manager = Instantiate(service, Vector3.zero, Quaternion.identity, transform).GetComponent<MonoBehaviour>();
            DS.SetSceneManager(manager);
            manager.GetComponent<IInSceneService>().Initialize();
        }
        OnInSceneManagersInitialized?.Invoke();
    }
    
    private void InitializeGameSettings()
    {
        SaveDataService.SaveMapSettings(globalSettings.mapSettings);
        SaveDataService.SavePlayerData(globalSettings.playerSettings);
        SaveDataService.SaveTerrainSettings(globalSettings.terrainSettings);
    }

    private void InitializeSceneService()
    {
        sceneInitializer.Initialize();
    }

    private void FinalizeLoading()
    {
        Debug.Log("Loading done");
    }
}

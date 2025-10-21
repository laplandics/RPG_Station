using UnityEngine;
using static EventManager;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] sceneServices;
    private SceneInitializeService _sceneInitializer;
    private GlobalDataManagerSo _globalDataManager;
    
    private void Start()
    {
        InitializeDS();
        OnSceneReady.AddListener(FinalizeLoading);

        DS.GetGlobalManager<GlobalInputsManagerSo>().DisableAllInputs();
        SpawnInSceneServices();
        InitializeSaveLoadServices();
        InitializeSceneInitializer();
    }
    
    private void InitializeDS() => DS.Initialize();

    private void SpawnInSceneServices()
    {
        foreach (var service in sceneServices)
        {
            var manager = Instantiate(service, Vector3.zero, Quaternion.identity, transform).GetComponent<MonoBehaviour>();
            DS.SetSceneManager(manager);
            if (manager.GetType() == typeof(SceneInitializeService)) _sceneInitializer = manager as SceneInitializeService;
            manager.GetComponent<IInSceneService>().Initialize();
        }
        OnInSceneManagersInitialized?.Invoke();
    }

    private void InitializeSaveLoadServices()
    {
        _globalDataManager = DS.GetGlobalManager<GlobalDataManagerSo>();
        _globalDataManager.ResetSettings();
        SaveLoadService.SubscribeToSaveLoadEvents();
    }
    
    private void InitializeSceneInitializer() => _sceneInitializer.InitializeGame();

    private void FinalizeLoading()
    {
        Debug.Log("Loading done");
        DS.GetGlobalManager<GlobalInputsManagerSo>().EnableAllInputs();
    }

    private void OnDestroy()
    {
        OnSceneReady.RemoveListener(FinalizeLoading);
        _globalDataManager.Dispose();
    }
}

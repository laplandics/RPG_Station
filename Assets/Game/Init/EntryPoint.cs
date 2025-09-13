using System.Collections;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private InitEssential initPrefabs;
    [SerializeField] private GameObject[] inSceneManagers;
    [SerializeField] private GlobalInputsState globalInputsState;
    private GameObject _cameraInstance;
    private GameObject _globalLightningInstance;
    
    private IEnumerator Start()
    {
        var isInitialized = false;
        
        DS.Initialize();
        DS.GetSoManager<GlobalInputsManagerSo>().BlockGlobalInputs();
        
        AssignManagersSoToInSceneManagersInitialization();
        SpawnInSceneManagers();
        SpawnUnityEssentials();
        
        DS.GetSoManager<GameManagerSo>().Initialize(_cameraInstance, _globalLightningInstance);
        
        isInitialized = true;
        yield return new WaitUntil(() => isInitialized);
        DS.GetSoManager<EventManagerSo>().onSceneInitializationCompleted?.Invoke();
        
        Debug.Log("Game Init Complete");
        DS.GetSoManager<GlobalInputsManagerSo>().AssignInputAction(globalInputsState);
    }

    private void AssignManagersSoToInSceneManagersInitialization()
    {
        foreach (var manager in DS.GetSoManagers())
        {
            if (manager is not IInSceneManagerListener rManager) continue;
            DS.GetSoManager<EventManagerSo>().onInSceneManagersInitialized.AddListener(rManager.OnSceneManagersInitialized);
        }
    }
    
    private void SpawnInSceneManagers()
    {
        foreach (var managerObject in inSceneManagers)
        {
            var manager = Instantiate(managerObject, Vector3.zero, Quaternion.identity).GetComponent<MonoBehaviour>();
            DS.SetSceneManager(manager);
            manager.GetComponent<IInSceneManager>().Initialize();
        }
        DS.GetSoManager<EventManagerSo>().onInSceneManagersInitialized?.Invoke();
    }

    private void SpawnUnityEssentials()
    {
        SpawnGlobalLight();
        SpawnCamera();
    }

    private void SpawnGlobalLight()
    {
        _globalLightningInstance = Instantiate(initPrefabs.globalLightning, initPrefabs.globalLightning.transform.position, Quaternion.identity);
    }

    private void SpawnCamera()
    {
        _cameraInstance = Instantiate(initPrefabs.camera, initPrefabs.camera.transform.position, Quaternion.identity);
    }
}

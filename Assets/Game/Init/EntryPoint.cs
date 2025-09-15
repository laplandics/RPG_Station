using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private InitEssential initPrefabs;
    [SerializeField] private GameObject[] inSceneManagers;
    private GameObject _cameraInstance;
    private GameObject _globalLightningInstance;

    private IEnumerator Start()
    {
        yield return StartCoroutine(InitializeDS());
        yield return StartCoroutine(AssignManagersSoToInSceneManagersInitialization());
        yield return StartCoroutine(SpawnInSceneManagers());
        yield return StartCoroutine(SpawnUnityEssentials());
        yield return StartCoroutine(InitializeGameManager());
        yield return new WaitForSeconds(0.1f);
        Debug.LogWarning("TODO: New Scene Load System");
        yield return StartCoroutine(CompleteInitialization());
    }

    private static IEnumerator InitializeDS()
    {
        DS.Initialize();
        yield return null;
        DS.GetSoManager<GlobalInputsManagerSo>().DisableAllGlobalInputs();
        yield return null;
    }

    private IEnumerator AssignManagersSoToInSceneManagersInitialization()
    {
        foreach (var manager in DS.GetSoManagers())
        {
            if (manager is not IInSceneManagerListener rManager) continue;
            DS.GetSoManager<EventManagerSo>().onInSceneManagersInitialized.AddListener(rManager.OnSceneManagersInitialized);
            yield return null;
        }
    }

    private IEnumerator SpawnInSceneManagers()
    {
        foreach (var managerObject in inSceneManagers)
        {
            var manager = Instantiate(managerObject, Vector3.zero, Quaternion.identity);
            DS.SetSceneManager(manager.GetComponent<MonoBehaviour>());
            manager.GetComponent<IInSceneManager>().Initialize();
            yield return null;
        }
        DS.GetSoManager<EventManagerSo>().onInSceneManagersInitialized?.Invoke();
    }

    private IEnumerator SpawnUnityEssentials()
    {
        _globalLightningInstance = Instantiate(initPrefabs.globalLightning, initPrefabs.globalLightning.transform.position, Quaternion.identity);
        yield return null;
        _cameraInstance = Instantiate(initPrefabs.camera, initPrefabs.camera.transform.position, Quaternion.identity);
        yield return null;
    }

    private IEnumerator InitializeGameManager()
    {
        yield return StartCoroutine(DS.GetSoManager<GameManagerSo>().Initialize(_cameraInstance, _globalLightningInstance));
        yield return null;
    }

    private static IEnumerator CompleteInitialization()
    {
        DS.GetSoManager<EventManagerSo>().onSceneInitializationCompleted?.Invoke();
        yield return null;
        DS.GetSoManager<GlobalInputsManagerSo>().EnableAllGlobalInputs();
        yield return null;
    }
}

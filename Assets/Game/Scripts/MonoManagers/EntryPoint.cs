using UnityEngine;
using static EventManager;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] inSceneManagers;

    private void Start()
    {

        InitializeDS();
        onSceneReady.AddListener(FinalizeLoading);

        DS.GetGlobalManager<GlobalInputsManagerSo>().DisableAllGlobalInputs();
        AssignManagersSoToInSceneManagersInitialization();
        SpawnInSceneManagers();
        DS.GetGlobalManager<GlobalInputsManagerSo>().EnableAllGlobalInputs();
        InitializeGameManager();
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
            onInSceneManagersInitialized.AddListener(rManager.OnSceneManagersInitialized);
        }
    }

    private void SpawnInSceneManagers()
    {
        foreach (var managerObject in inSceneManagers)
        {
            var manager = Instantiate(managerObject, Vector3.zero, Quaternion.identity, transform);
            DS.SetSceneManager(manager.GetComponent<MonoBehaviour>());
            manager.GetComponent<IInSceneManager>().Initialize();
        }
        onInSceneManagersInitialized?.Invoke();
    }

    private void InitializeGameManager()
    {
        DS.GetGlobalManager<GameManagerSo>().Initialize();
    }

    private void FinalizeLoading()
    {
        Debug.Log("Loading done");
    }
}

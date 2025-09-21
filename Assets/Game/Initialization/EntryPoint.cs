using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] inSceneManagers;

    private void Start()
    {
        InitializeDS();
        AssignManagersSoToInSceneManagersInitialization();
        SpawnInSceneManagers();
        DS.GetSoManager<GlobalInputsManagerSo>().EnableAllGlobalInputs();
        InitializeGameManager();
            
        DS.GetSoManager<EventManagerSo>().onManagersInitialized.Invoke();
    }

    private void InitializeDS()
    {
        DS.Initialize();
        DS.GetSoManager<GlobalInputsManagerSo>().DisableAllGlobalInputs();
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
            var manager = Instantiate(managerObject, Vector3.zero, Quaternion.identity, transform);
            DS.SetSceneManager(manager.GetComponent<MonoBehaviour>());
            manager.GetComponent<IInSceneManager>().Initialize();
        }
        DS.GetSoManager<EventManagerSo>().onInSceneManagersInitialized?.Invoke();
    }

    private void InitializeGameManager()
    {
        DS.GetSoManager<GameManagerSo>().Initialize();
    }
}

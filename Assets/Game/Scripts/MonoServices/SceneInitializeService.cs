using System.Collections;
using UnityEngine;
using static EventManager;

public class SceneInitializeService : MonoBehaviour, IInSceneService
{
    [SerializeField] private ScriptableObject[] objectSpawners;
    private RoutineService _routineService;
    private bool _isDataInitialized;

    private CameraInitializer CameraInitializer { get; set; }
    private LightInitializer LightInitializer { get; set; }
    public MapInitializer MapInitializer { get; private set; }
    public PlayerInitializer PlayerInitializer { get; private set; }
    private UIInitializer UIInitializer { get; set; }

    public void Initialize()
    {
        OnLoad.AddListener(() => StartCoroutine(RestartGame()));
    }
    
    public void SetDataState(bool state) => _isDataInitialized = state;

    private IEnumerator RestartGame()
    {
        yield return new WaitUntil(() => _isDataInitialized);
        yield return null;
        InitializeGame();
        DS.GetGlobalManager<GlobalInputsManagerSo>().EnableAllInputs();
    }

    public void InitializeGame()
    {
        AssignSceneGameObjectInitializers();
        AssignSpawnersSo();
    }
    
    private void AssignSceneGameObjectInitializers()
    {
        CameraInitializer = new CameraInitializer();
        LightInitializer = new LightInitializer();
        MapInitializer = new MapInitializer();
        PlayerInitializer = new PlayerInitializer();
        UIInitializer = new UIInitializer();
    }
    
    private void AssignSpawnersSo()
    {
        foreach (var objectSpawner in objectSpawners)
        {
            if (objectSpawner is not ISpawner spawner) continue;
            spawner.InitializeSpawner();
        }
    }
    
    public void EraseScene()
    {
        CameraInitializer.DeInitialize();
        LightInitializer.DeInitialize();
        MapInitializer.DeInitialize();
        PlayerInitializer.DeInitialize();
        UIInitializer.DeInitialize();
        
        CameraInitializer = null;
        LightInitializer = null;
        MapInitializer = null;
        PlayerInitializer = null;
        UIInitializer = null;
    }
}
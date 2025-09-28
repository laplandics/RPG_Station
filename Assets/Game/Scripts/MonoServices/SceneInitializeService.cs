using UnityEngine;
using static EventManager;

public class SceneInitializeService : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] objectSpawners;
    
    private CameraInitializer _cameraInitializer;
    private LightInitializer _lightInitializer;
    private MapInitializer _mapInitializer;
    private PlayerInitializer _playerInitializer;
    private UIInitializer _uiInitializer;
    
    public void Initialize()
    {
        OnLoad.AddUniqueListener(Initialize);
        AssignSceneGameObjectInitializers();
        AssignSpawnersSo();
    }
    
    private void AssignSceneGameObjectInitializers()
    {
        _cameraInitializer = new CameraInitializer();
        _lightInitializer = new LightInitializer();
        _mapInitializer = new MapInitializer();
        _playerInitializer = new PlayerInitializer();
        _uiInitializer = new UIInitializer();
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
        
    }
}
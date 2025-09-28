using UnityEngine.Rendering.Universal;
using static EventManager;

public class LightInitializer
{
    private Light2D _globalLight;
    private readonly GameObjectsService _gameObjectsService;

    public LightInitializer()
    {
        OnLightSpawned.AddUniqueListener(SetLight);
        _gameObjectsService = DS.GetSceneManager<GameObjectsService>();
    }

    private void SetLight(Light2D light, SaveData _)
    {
        _globalLight = light;
    }
    
    public void DeInitialize() => _gameObjectsService.Despawn(_globalLight.transform.parent.gameObject);
}

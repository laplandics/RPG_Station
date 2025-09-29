using UnityEngine.Rendering.Universal;
using static EventManager;

public class LightInitializer
{
    private Light2D _globalLight;
    private readonly GOService _goService;

    public LightInitializer()
    {
        OnLightSpawned.AddListener(SetLight);
        _goService = DS.GetSceneManager<GOService>();
    }

    private void SetLight(Light2D light, SaveData _)
    {
        _globalLight = light;
    }

    public void DeInitialize()
    {
        _goService.Despawn(_globalLight.transform.parent.gameObject);
        OnLightSpawned.RemoveListener(SetLight);
    }
}

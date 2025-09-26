using UnityEngine;
using UnityEngine.Rendering.Universal;
using static EventManager;

[CreateAssetMenu(fileName = "LightManager", menuName = "ManagersSO/LightManager")]
public class LightManagerSo : ScriptableObject, IObjectManagerSo
{
    public const string LIGHT_KEY = "LIGHT";
    private Light2D _globalLight;

    public string Key => LIGHT_KEY;

    public SaveData Initialize()
    {
        onLightSpawned.AddUniqueListener(SetLight);
        return new SaveData { instanceKey = LIGHT_KEY };
    }

    private void SetLight(Light2D light, SaveData _)
    {
        _globalLight = light;
    }
    
    public void SetNewData(SaveData _){}

    public SaveData GetCurrentData() => new() { instanceKey = LIGHT_KEY };

    public void DestroyCurrentInstance() => Destroy(_globalLight.transform.parent.gameObject);
}

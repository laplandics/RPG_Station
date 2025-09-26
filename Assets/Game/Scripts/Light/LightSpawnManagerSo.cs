using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static EventManager;

[CreateAssetMenu(fileName = "LightSpawner", menuName = "SpawnersSO/LightSpawner")]
public class LightSpawnManagerSo : ScriptableObject, ISpawner
{
    [SerializeField] private GameObject _lightPrefab;
    private SaveData _lightData;

    public bool TryInitializeSpawner(SaveData data)
    {
        if (data.instanceKey != LightManagerSo.LIGHT_KEY) return false;
        _lightData = data;
        DS.GetSceneManager<RoutineManager>().StartRoutine(SpawnLight());
        return true;
    }

    private IEnumerator SpawnLight()
    {
        var light = Instantiate(_lightPrefab, Vector2.zero, Quaternion.identity);
        light.name = _lightData.instanceKey;
        var gameLight = light.GetComponentInChildren<Light2D>();
        onLightSpawned?.Invoke(gameLight, _lightData);
        yield break;
    }
}

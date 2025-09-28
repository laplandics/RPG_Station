using UnityEngine;
using UnityEngine.Rendering.Universal;
using static EventManager;

[CreateAssetMenu(fileName = "LightSpawner", menuName = "SpawnersSO/LightSpawner")]
public class LightSpawnerSo : ScriptableObject, ISpawner
{
    [SerializeField] private GameObject lightPrefab;

    public void InitializeSpawner() => SpawnLight();

    private void SpawnLight()
    {
        var light = Instantiate(lightPrefab, Vector2.zero, Quaternion.identity);
        light.name = "LIGHT";
        var gameLight = light.GetComponentInChildren<Light2D>();
        OnLightSpawned?.Invoke(gameLight, new SaveData{instanceKey = "LIGHT"});
    }
}

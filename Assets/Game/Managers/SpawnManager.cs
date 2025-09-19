using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnManager : MonoBehaviour, IInSceneManager
{
    [SerializeField] private MainGameObjectsSo instances;
    [SerializeField] private int initializeOrder;
    private EventManagerSo eventManager;
    public int InitializeOrder => initializeOrder;

    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        eventManager = DS.GetSoManager<EventManagerSo>();
        eventManager.onManagersInitialized.AddListener(() => DS.GetSceneManager<RoutineManager>().StartRoutine(StartSpawningSceneObjects()));
    }

    private T Spawn<T>(Vector3 position, Transform parent = null)
    {
        var instance = GetProperInstance<T>();
        if (instance.TryGetComponent<T>(out _)) return Instantiate(instance, position, Quaternion.identity, parent).GetComponent<T>();
        else if (instance.GetComponentInChildren<T>() != null) return Instantiate(instance, position, Quaternion.identity, parent).GetComponentInChildren<T>();

        return default;
    }

    private GameObject GetProperInstance<T>()
    {
        foreach (var obj in instances.allPrefabs)
        {
            if (obj.GetComponent<T>() == null && obj.GetComponentInChildren<T>() == null) continue;
            return obj;
        }
        return null;
    }

    private IEnumerator StartSpawningSceneObjects()
    {
        var routineManager = DS.GetSceneManager<RoutineManager>();
        yield return routineManager.StartRoutine(SpawnUnityEssentials());
        yield return routineManager.StartRoutine(SpawnPlayer());
        yield return routineManager.StartRoutine(SpawnMap());

        eventManager.onSceneReady?.Invoke();
        yield return null;
    }

    private IEnumerator SpawnUnityEssentials()
    {
        var light = Spawn<Light2D>(Vector3.zero);
        var camera = Spawn<Camera>(Vector3.zero);
        camera.GetComponentInParent<GameCamera>().Initialize();
        eventManager.onUnityEssentialsSpawned?.Invoke(light, camera);
        yield break;
    }
    
    private IEnumerator SpawnPlayer()
    {
        var player = Spawn<Player>(Vector3.zero);
        eventManager.onPlayerSpawned?.Invoke(player);
        yield break;
    }

    private IEnumerator SpawnMap()
    {
        var map = Spawn<Map>(Vector3.zero);
        eventManager.onMapSpawned?.Invoke(map);
        yield break;
    }
}

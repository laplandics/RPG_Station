using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DS
{
    private static readonly Dictionary<Type, ScriptableObject> ManagersSo = new();
    private static readonly Dictionary<Type, ScriptableObject> SpawnersSo = new();
    private static readonly Dictionary<Type, MonoBehaviour> ManagersMb = new();

    private static bool _isInitialized;

    public static void Initialize()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        var managersSo = Resources.LoadAll<ScriptableObject>("ManagersSO");
        foreach (var manager in managersSo)
        {
            var type = manager.GetType();
            ManagersSo.TryAdd(type, manager);
        }

        var spawnersSo = Resources.LoadAll<ScriptableObject>("SpawnersSO");
        foreach (var spawner in spawnersSo)
        {
            var type = spawner.GetType();
            SpawnersSo.TryAdd(type, spawner);
        }
    }

    public static void SetSceneManager<T>(T manager) where T : MonoBehaviour
    {
        if (!_isInitialized) return;
        var type = manager.GetType();
        ManagersMb.TryAdd(type, manager);
    }

    public static T GetSoManager<T>() where T : ScriptableObject
    {
        if (!_isInitialized) return null;
        if (ManagersSo.TryGetValue(typeof(T), out var manager)) return manager as T;
        return null;
    }

    public static T GetSoSpawner<T>() where T : ScriptableObject
    {
        if (!_isInitialized) return null;
        if (SpawnersSo.TryGetValue(typeof(T), out var spawner)) return spawner as T;
        return null;
    }

    public static T GetSceneManager<T>() where T : MonoBehaviour
    {
        if (!_isInitialized) return null;
        if (ManagersMb.TryGetValue(typeof(T), out var manager)) return manager as T;
        return null;
    }

    public static ScriptableObject[] GetSoManagers()
    {
        if (!_isInitialized) return null;
        return ManagersSo.Values.ToArray();
    }

    public static ScriptableObject[] GetSoSpawners()
    {
        if (!_isInitialized) return null;
        return SpawnersSo.Values.ToArray();
    }
}

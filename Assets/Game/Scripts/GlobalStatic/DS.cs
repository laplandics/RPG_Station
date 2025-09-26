using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DS
{
    private static readonly Dictionary<Type, ScriptableObject> GlobalManagers = new();
    private static readonly Dictionary<Type, ScriptableObject> ObjectManagersSo = new();
    private static readonly Dictionary<Type, MonoBehaviour> ManagersMb = new();

    private static bool _isInitialized;

    public static void Initialize()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        var globalManagers = Resources.LoadAll<ScriptableObject>("ManagersSO");
        foreach (var manager in globalManagers)
        {
            var type = manager.GetType();
            GlobalManagers.TryAdd(type, manager);
        }

        var objectManagers = Resources.LoadAll<ScriptableObject>("ObjectsManagersSO");
        foreach (var manager in objectManagers)
        {
            if (manager is not IObjectManagerSo) continue;
            var type = manager.GetType();
            ObjectManagersSo.TryAdd(type, manager);
        }
    }

    public static T GetGlobalManager<T>() where T : ScriptableObject
    {
        if (!_isInitialized) return null;
        if (GlobalManagers.TryGetValue(typeof(T), out var manager)) return manager as T;
        return null;
    }
    public static ScriptableObject[] GetGlobalManagers()
    {
        if (!_isInitialized) return null;
        return GlobalManagers.Values.ToArray();
    }
    public static T GetObjectManager<T>() where T : ScriptableObject
    {
        if (!_isInitialized) return null;
        if (ObjectManagersSo.TryGetValue(typeof(T), out var manager)) return manager as T;
        return null;
    }
    public static ScriptableObject[] GetObjectManagers()
    {
        if (!_isInitialized) return null;
        return ObjectManagersSo.Values.ToArray();
    }

    public static void SetSceneManager<T>(T manager) where T : MonoBehaviour
    {
        if (!_isInitialized) return;
        var type = manager.GetType();
        ManagersMb.TryAdd(type, manager);
    }

    public static T GetSceneManager<T>() where T : MonoBehaviour
    {
        if (!_isInitialized) return null;
        if (ManagersMb.TryGetValue(typeof(T), out var manager)) return manager as T;
        return null;
    }

}

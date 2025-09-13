using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DS
{
    private static readonly Dictionary<Type, ScriptableObject> ManagersSo = new();
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
            if (ManagersSo.TryAdd(type, manager)) {Debug.Log($"Adding manager {type.Name}");}
        }
    }

    public static T GetManager<T>() where T : ScriptableObject
    {
        if (!_isInitialized) return null;
        if (ManagersSo.TryGetValue(typeof(T), out var manager)) return manager as T;
        Debug.LogError($"Can't find manager {typeof(T).Name}");
        return null;
    }
    
    public static T GetInSceneManager<T>() where T : MonoBehaviour
    {
        if (!_isInitialized) return null;
        if (ManagersMb.TryGetValue(typeof(T), out var manager)) return manager as T;
        Debug.LogError($"Can't find manager {typeof(T).Name}");
        return null;
    }

    public static void SetManager<T>(T manager) where T : MonoBehaviour
    {
        if (!_isInitialized) return;
        var type = manager.GetType();
        if (ManagersMb.TryAdd(type, manager)) {Debug.Log($"Adding manager {type.Name}");}
    }

    public static ScriptableObject[] GetAllManagers()
    {
        if (!_isInitialized) return null;
        return ManagersSo.Values.ToArray();
    }
}

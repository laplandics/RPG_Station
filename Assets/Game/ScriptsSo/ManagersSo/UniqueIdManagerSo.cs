using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UniqueIdManager", menuName = "ManagersSO/UniqueIdManager")]
public class UniqueIdManagerSo : ScriptableObject
{
    [Header("Persistent ID's")]
    [SerializeField] private List<GameObject> persistentPrefabs;
    
    
    private void OnValidate()
    {
        foreach (var obj in persistentPrefabs)
        {
            if (obj.GetComponent<MonoBehaviour>() is not ISaveAble saveAble) continue;
            saveAble.PrefabKey = Guid.NewGuid().ToString();
        }
    }
}


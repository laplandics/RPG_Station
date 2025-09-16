using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UniqueIdManager", menuName = "ManagersSO/UniqueIdManager")]
public class UniqueIdManagerSo : ScriptableObject
{
    [SerializeField] private bool regenerateIds;
    [Header("Persistent ID's")]
    [SerializeField] private List<GameObject> persistentPrefabs;
    
    
    private void OnValidate()
    {
        GenerateUniqueId();
        regenerateIds = false;
    }

    private void GenerateUniqueId()
    {
        if (!regenerateIds) return;
        foreach (var obj in persistentPrefabs)
        {
            if (obj.GetComponent<MonoBehaviour>() is not ISaveAble saveAble) continue;
            if (string.IsNullOrEmpty(saveAble.PrefabKey))
            {
                saveAble.PrefabKey = Guid.NewGuid().ToString();
            }
        }
    }
}


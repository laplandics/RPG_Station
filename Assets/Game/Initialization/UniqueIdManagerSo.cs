using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "UniqueIdManager", menuName = "ManagersSO/UniqueIdManager")]
public class UniqueIdManagerSo : ScriptableObject
{
    [SerializeField] private bool regenerateIds;
    [Header("Persistent ID's")]
    [SerializeField] private List<GameObject> persistentPrefabs;
    [Header("Chunk Prefabs")]
    [SerializeField] private List<Chunk> chunks;
    [Header("Enemy Prefabs")]
    [SerializeField] private List<Enemy> enemies;
    
    
    private void OnValidate()
    {
        GenerateUniqueId();
        regenerateIds = false;
    }
    
#if UNITY_EDITOR
    private void GenerateUniqueId()
    {
        if (!regenerateIds) return;
        foreach (var obj in persistentPrefabs)
        {
            if (!obj) continue;
            if (obj.GetComponent<MonoBehaviour>() is not ISaveAble saveAble) continue;
            if (string.IsNullOrEmpty(saveAble.PrefabKey))
            {
                saveAble.PrefabKey = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(obj);
            }
        }

        foreach (var chunk in chunks)
        {
            if (string.IsNullOrEmpty(chunk.PrefabKey))
            {
                chunk.PrefabKey = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(chunk);
            }
        }

        foreach (var enemy in enemies)
        {
            if (string.IsNullOrEmpty(enemy.PrefabKey))
            {
                enemy.PrefabKey = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(enemy);
            }
        }
    }

    private void OnEnable()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}


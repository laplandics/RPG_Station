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
    [Header("EnemyMod Prefabs")]
    [SerializeField] private List<ScriptableObject> mods;
    
    
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
            if (string.IsNullOrEmpty(saveAble.InstanceKey))
            {
                saveAble.InstanceKey = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(obj);
            }
        }

        foreach (var chunk in chunks)
        {
            if (string.IsNullOrEmpty(chunk.InstanceKey))
            {
                chunk.InstanceKey = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(chunk);
            }
        }

        foreach (var enemy in enemies)
        {
            if (string.IsNullOrEmpty(enemy.InstanceKey))
            {
                enemy.InstanceKey = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(enemy);
            }
        }

        foreach (var mod in mods)
        {
            if (mod is not IEnemyModSetter setter) continue;
            if (string.IsNullOrEmpty(setter.ModKey))
            {
                setter.ModKey = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(mod);
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


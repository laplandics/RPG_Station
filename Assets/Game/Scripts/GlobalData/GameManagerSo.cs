using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;


[CreateAssetMenu(fileName = "GameManager", menuName = "ManagersSO/GameManager")]
public class GameManagerSo : ScriptableObject
{
    [SerializeField] private ScriptableObject[] objectSpawners;
    private List<IObjectManagerSo> _objectManagers = new();
    private List<SaveData> _saveDatas = new();

    public void Initialize()
    {
        _objectManagers.Clear();
        _saveDatas.Clear();

        onSave.AddListener(SaveInstances);
        onLoad.AddListener(() => DS.GetSceneManager<RoutineManager>().StartRoutine(LoadInstances()));

        InitializeManagers();
        InitializeSpawners();
    }

    private void InitializeManagers()
    {
        foreach (var manager in DS.GetObjectManagers())
        {
            if (manager is not IObjectManagerSo objManager) continue;
            _objectManagers.Add(objManager);
            _saveDatas.Add(objManager.Initialize());
        }
    }
    private void InitializeSpawners()
    {
        foreach (var so in objectSpawners)
        {
            if (so is not ISpawner spawner) continue;
            foreach (var data in _saveDatas)
            {
                if (!spawner.TryInitializeSpawner(data)) continue;
            }
        }
    }

    private void SaveInstances()
    {
        _saveDatas.Clear();
        foreach (var objManager in _objectManagers)
        {
            _saveDatas.Add(objManager.GetCurrentData());
        }
        foreach (var data in _saveDatas)
        {
            DS.GetGlobalManager<SaveLoadManagerSo>().Save(data.instanceKey, data);
        }
    }

    private IEnumerator LoadInstances()
    {
        _saveDatas.Clear();
        foreach (var objManager in _objectManagers)
        {
            _saveDatas.Add(DS.GetGlobalManager<SaveLoadManagerSo>().Load<SaveData>(objManager.Key));
        }

        foreach (var objManager in _objectManagers)
        {
            foreach (var data in _saveDatas)
            {
                if (objManager.Key != data.instanceKey) continue;
                objManager.SetNewData(data);
                break;
            }

            objManager.DestroyCurrentInstance();
            yield return null;
        }

        InitializeSpawners();
    }
}

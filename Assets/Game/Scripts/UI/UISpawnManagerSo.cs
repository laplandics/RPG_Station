using System.Collections;
using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "UISpawner", menuName = "SpawnersSO/UISpawner")]
public class UISpawnManagerSo : ScriptableObject, ISpawner
{
    [SerializeField] private Terminal uiPrefab;
    private SaveData _uiData;
    public bool TryInitializeSpawner(SaveData data)
    {
        _uiData = data;
        if (data.instanceKey != UIManagerSo.UI_KEY) return false;
        DS.GetSceneManager<RoutineManager>().StartRoutine(SpawnUi());
        return true;
    }

    private IEnumerator SpawnUi()
    {
        var terminal = Instantiate(uiPrefab, Vector2.zero, Quaternion.identity);
        terminal.gameObject.name = _uiData.instanceKey;
        onTerminalSpawned?.Invoke(terminal, _uiData);
        yield break;
    }
}
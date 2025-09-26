using System.Collections;
using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "PlayerSpawner", menuName = "SpawnersSO/PlayerSpawner")]
public class PlayerSpawnManagerSo : ScriptableObject, ISpawner
{
    [SerializeField] private Player _playerPrefab;
    private PlayerData _playerData;

    public bool TryInitializeSpawner(SaveData data)
    {
        if (data is not PlayerData playerData) return false;
        _playerData = playerData;
        DS.GetSceneManager<RoutineManager>().StartRoutine(SpawnMap());
        return true;
    }

    private IEnumerator SpawnMap()
    {
        var player = Instantiate(_playerPrefab, new Vector2(_playerData.x, _playerData.y), Quaternion.identity);
        player.gameObject.name = _playerData.instanceKey;
        onPlayerSpawned?.Invoke(player, _playerData);
        yield break;
    }
}
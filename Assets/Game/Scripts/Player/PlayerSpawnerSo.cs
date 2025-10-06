using UnityEngine;
using static EventManager;
using static GameDataInjector;

[CreateAssetMenu(fileName = "PlayerSpawner", menuName = "SpawnersSO/PlayerSpawner")]
public class PlayerSpawnerSo : ScriptableObject, ISpawner
{
    [SerializeField] private Player playerPrefab;
    private PlayerData _playerData;

    public void InitializeSpawner()
    {
        _playerData = InjectPlayerData;
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab, new Vector2(_playerData.x, _playerData.y), Quaternion.identity);
        player.gameObject.name = _playerData.instanceKey;
        OnPlayerSpawned?.Invoke(player, _playerData);
    }
}
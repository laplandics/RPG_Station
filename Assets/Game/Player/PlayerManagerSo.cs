using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerManager", menuName = "ManagersSO/PlayerManager")]
public class PlayerManagerSo : ScriptableObject
{
    public Vector2 MoveInput => _playerController.GetMoveInput();
    private PlayerSpriteSwapper _playerSpriteSwapper;
    private Player _player;
    private PlayerController _playerController;

    public Task Initialize()
    {
        DS.GetSoManager<EventManagerSo>().onPlayerSpawned.AddListener(InitializePlayer);
        return Task.CompletedTask;
    }

    public void InitializePlayer(Player player)
    {
        _player = player;
        _playerController = _player.GetController();
        _playerSpriteSwapper = _player.GetComponent<PlayerSpriteSwapper>();
        _playerSpriteSwapper.Initialize();
        DS.GetSoManager<EventManagerSo>().onSceneReady.AddListener(InitializePlayerController);
    }

    private void InitializePlayerController()
    {
        _playerController.Initialize();
        DS.GetSceneManager<RoutineManager>().GetUpdateAction(_playerSpriteSwapper.SetPlayerSprite);
    }

    public async Task LoadPlayerData()
    {
        await _player.Load();
    }

    public async Task SavePlayerData()
    {
        await _player.Save();
    }

    public Transform GetPlayerTransform() => _player.transform;
}
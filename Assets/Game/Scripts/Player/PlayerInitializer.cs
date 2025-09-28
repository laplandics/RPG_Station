using UnityEngine;
using static EventManager;

public class PlayerInitializer
{
    private readonly GameObjectsService _gameObjectsService;
    private PlayerChunkObserver _playerChunkObserver;
    private PlayerSpriteSwapper _playerSpriteSwapper;
    private PlayerController _playerController;
    private Player _player;
    public Vector2 MoveInput => _playerController.GetMoveInput();

    public PlayerInitializer()
    {
        OnPlayerSpawned.AddUniqueListener(SetPlayer);
        _gameObjectsService = DS.GetSceneManager<GameObjectsService>();
    }

    private void SetPlayer(Player player, SaveData _)
    {
        _player = player;
        _playerChunkObserver = new PlayerChunkObserver(player.transform);
        _playerController = _player.GetComponent<PlayerController>();
        _playerSpriteSwapper = _player.GetComponent<PlayerSpriteSwapper>();
        OnSceneReady.AddUniqueListener(InitializePlayerScripts);
    }

    private void InitializePlayerScripts()
    {
        _playerController.Initialize();
        _playerSpriteSwapper.Initialize(this);
    }

    public void DeInitialize() => _gameObjectsService.Despawn(_player.gameObject);
}
using UnityEngine;
using static EventManager;

public class PlayerInitializer
{
    private readonly GOService _goService;
    private PlayerChunkObserver _playerChunkObserver;
    private PlayerSpriteSwapper _playerSpriteSwapper;
    private PlayerController _playerController;
    private Player _player;
    public Vector2 MoveInput => _playerController.GetMoveInput();

    private PlayerData _playerData;
    public PlayerData CurrentPlayerData
    {
        get
        {
            _playerData.x = (int)_player.transform.position.x;
            _playerData.y = (int)_player.transform.position.y;
            return _playerData;
        }
        private set => _playerData = value;
    }

    public PlayerInitializer()
    {
        OnPlayerSpawned.AddListener(SetPlayer);
        _goService = DS.GetSceneManager<GOService>();
    }

    private void SetPlayer(Player player, PlayerData data)
    {
        CurrentPlayerData = data;
        _player = player;
        _playerChunkObserver = new PlayerChunkObserver(player.transform);
        _playerController = _player.GetComponent<PlayerController>();
        _playerSpriteSwapper = _player.GetComponent<PlayerSpriteSwapper>();
        OnSceneReady.AddListener(InitializePlayerScripts);
    }

    private void InitializePlayerScripts()
    {
        _playerController.Initialize();
        _playerSpriteSwapper.Initialize(this);
    }

    public void DeInitialize()
    {
        _goService.Despawn(_player.gameObject);
        CurrentPlayerData = null;
        OnSceneReady.RemoveListener(InitializePlayerScripts);
        OnPlayerSpawned.RemoveListener(SetPlayer);
        _playerChunkObserver.Dispose();
    }
}
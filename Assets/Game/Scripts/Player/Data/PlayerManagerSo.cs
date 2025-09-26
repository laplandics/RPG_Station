using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "PlayerManager", menuName = "ManagersSO/PlayerManager")]
public class PlayerManagerSo : ScriptableObject, IObjectManagerSo
{
    public const string PLAYER_KEY = "PLAYER";
    public Vector2 MoveInput => _playerController.GetMoveInput();
    private PlayerSpriteSwapper _playerSpriteSwapper;
    private Player _player;
    private PlayerController _playerController;

    public PlayerData PlayerData { get; set; }
    public string Key => PLAYER_KEY;
    public SaveData Initialize()
    {
        onPlayerSpawned.AddUniqueListener(InitializePlayer);
        return new PlayerData { instanceKey = PLAYER_KEY, x = 0, y = 0 };
    }

    public void InitializePlayer(Player player, SaveData _)
    {
        _player = player;
        _playerController = _player.GetComponent<PlayerController>();
        _playerSpriteSwapper = _player.GetComponent<PlayerSpriteSwapper>();
        onSceneReady.AddUniqueListener(InitializePlayerScripts);
    }

    private void InitializePlayerScripts()
    {
        _playerController.Initialize();
        _playerSpriteSwapper.Initialize();
    }

    public SaveData GetCurrentData() => new PlayerData
    {
        instanceKey = PLAYER_KEY,
        x = (int)_player.transform.position.x,
        y = (int)_player.transform.position.y
    };

    public void SetNewData(SaveData newData)
    {
        if (newData is not PlayerData playerData) return;
        _player.transform.position = new Vector2(playerData.x, playerData.y);
    }

    public void DestroyCurrentInstance() => Destroy(_player.gameObject);
}
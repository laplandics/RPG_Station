using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerManager", menuName = "ManagersSO/PlayerManager")]
public class PlayerManagerSo : ScriptableObject
{
    public Vector2 MoveInput => _playerController.GetMoveInput();
    private PlayerSpriteSwapper _playerSpriteSwapper;
    private Player _player;
    private PlayerController _playerController;
    
    public Task<Player> SpawnPlayer(GameObject playerPrefab)
    {
        var playerInstance = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        _player = playerInstance.GetComponent<Player>();
        _playerController = _player.GetController();
        _playerSpriteSwapper = _player.GetComponent<PlayerSpriteSwapper>();
        _playerSpriteSwapper.Initialize();
        DS.GetSoManager<EventManagerSo>().onSceneInitializationCompleted.AddListener(InitializePlayerController);
        
        return Task.FromResult(_player);
    }

    private void InitializePlayerController()
    {
        _playerController.Initialize();
        DS.GetSceneManager<RoutineManager>().GetUpdateAction(_playerSpriteSwapper.SetPlayerSprite);
    }

    public Task LoadPlayerData()
    {
        _player.Load();
        return Task.CompletedTask;
    }

    public void SavePlayerData() => _player.Save();
    
    public Vector2 GetPlayerPosition() => _player.transform.position;
}
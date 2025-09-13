using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerManager", menuName = "ManagersSO/PlayerManager")]
public class PlayerManagerSo : ScriptableObject
{
    [SerializeField] private string playerKey;
    private Player _player;
    private PlayerController _playerController;
    
    public Player SpawnPlayer(GameObject playerPrefab)
    {
        var playerInstance = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        _player = playerInstance.GetComponent<Player>();
        _playerController = _player.GetController();
        DS.GetSoManager<EventManagerSo>().onSceneInitializationCompleted.AddListener(InitializePlayerController);
        
        return _player;
    }

    private void InitializePlayerController()
    {
        _playerController.Initialize();
        DS.GetSceneManager<RoutineManager>().GetFixedUpdateAction(_playerController.MovePlayer);
    }

    public void LoadPlayerData()
    {
        DS.GetSoManager<SaveLoadManagerSo>().Load<PlayerData>(playerKey, data =>
        {
            _player.transform.position = data.position;
        });
    }

    public void SavePlayerData()
    {
        var data = new PlayerData
        {
            position = _player.transform.position,
        };
        DS.GetSoManager<SaveLoadManagerSo>().Save(playerKey, data);
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
}
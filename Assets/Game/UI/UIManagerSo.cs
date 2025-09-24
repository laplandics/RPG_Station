using UnityEngine;

[CreateAssetMenu(fileName = "UIManager", menuName = "ManagersSO/UIManager")]
public class UIManagerSo : ScriptableObject
{
    private Terminal _terminal;
    private EventManagerSo _eventManager;

    public void Initialize()
    {
        _eventManager = DS.GetSoManager<EventManagerSo>();
        _eventManager.onTerminalSpawned.AddListener(InitializeTerminal);
    }

    private void InitializeTerminal(Terminal terminal)
    {
        _terminal = terminal;
        _eventManager.onChunkSpawned.AddListener((_, _) => terminal.UpdateChunksCount(true));
        _eventManager.onChunkDespawned.AddListener((_, _, _) => terminal.UpdateChunksCount(false));
        _eventManager.onEnemySpawned.AddListener((_, _) => terminal.UpdateEnemiesCount(true));
        _eventManager.onEnemyDespawned.AddListener((_, _) => terminal.UpdateEnemiesCount(false));
        _eventManager.onPlayerSpawned.AddListener(player => terminal.UpdatePlayerPosition(player.transform.position));
        _eventManager.onPlayersPositionChanged.AddListener(playerTransform => terminal.UpdatePlayerPosition(playerTransform.position));
    }
}

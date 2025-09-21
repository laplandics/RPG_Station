using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "UIManager", menuName = "ManagersSO/UIManager")]
public class UIManagerSo : ScriptableObject
{
    private Terminal _terminal;
    private EventManagerSo _eventManager;

    public Task Initialize()
    {
        _eventManager = DS.GetSoManager<EventManagerSo>();
        _eventManager.onTerminalSpawned.AddListener(InitializeTerminal);
        return Task.CompletedTask;
    }

    private void InitializeTerminal(Terminal terminal)
    {
        _terminal = terminal;
        _eventManager.onChunkSpawned.AddListener(_ => terminal.UpdateChunksCount(true));
        _eventManager.onChunkDespawned.AddListener(_ => terminal.UpdateChunksCount(false));
        _eventManager.onEnemySpawned.AddListener(_ => terminal.UpdateEnemiesCount(true));
        _eventManager.onEnemyDespawned.AddListener(_ => terminal.UpdateEnemiesCount(false));
        _eventManager.onPlayerSpawned.AddListener(player => terminal.UpdatePlayerPosition(player.transform.position));
        _eventManager.onPlayersPositionChanged.AddListener(playerTransform => terminal.UpdatePlayerPosition(playerTransform.position));
    }
}

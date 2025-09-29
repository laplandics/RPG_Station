using UnityEngine;
using static EventManager;

public class UIInitializer
{
    private Terminal _terminal;
    private readonly GOService _goService;

    public UIInitializer()
    {
        OnTerminalSpawned.AddListener(SetTerminal);
        _goService = DS.GetSceneManager<GOService>();
    }

    private void SetTerminal(Terminal terminal, SaveData _)
    {
        _terminal = terminal;
        OnPlayerSpawned.AddListener(UpdatePlayerInformation);
        OnPlayersPositionChanged.AddListener(UpdatePlayerInformation);
    }

    private void UpdatePlayerInformation(Player player, PlayerData _) => _terminal.UpdatePlayerPosition(player.transform.position);
    private void UpdatePlayerInformation(Transform playerTransform) => _terminal.UpdatePlayerPosition(playerTransform.position);
    
    
    public void DeInitialize()
    {
        _goService.Despawn(_terminal.gameObject);
        OnPlayerSpawned.RemoveListener(UpdatePlayerInformation);
        OnPlayersPositionChanged.RemoveListener(UpdatePlayerInformation);
        OnTerminalSpawned.RemoveListener(SetTerminal);
    }
}

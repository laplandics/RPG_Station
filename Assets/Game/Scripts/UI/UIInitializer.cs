using static EventManager;

public class UIInitializer
{
    private Terminal _terminal;
    private readonly GameObjectsService _gameObjectsService;

    public UIInitializer()
    {
        OnTerminalSpawned.AddUniqueListener(SetTerminal);
        _gameObjectsService = DS.GetSceneManager<GameObjectsService>();
    }

    private void SetTerminal(Terminal terminal, SaveData _)
    {
        _terminal = terminal;
        OnPlayerSpawned.AddUniqueListener((player, _) => _terminal.UpdatePlayerPosition(player.transform.position));
        OnPlayersPositionChanged.AddUniqueListener(playerTransform => _terminal.UpdatePlayerPosition(playerTransform.position));
    }

    public void DestroyCurrentInstance() => _gameObjectsService.Despawn(_terminal.gameObject);
}

using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "UIManager", menuName = "ManagersSO/UIManager")]
public class UIManagerSo : ScriptableObject, IObjectManagerSo
{
    public const string UI_KEY = "UI";
    private Terminal _terminal;

    public string Key => UI_KEY;

    public SaveData Initialize()
    {
        onTerminalSpawned.AddUniqueListener(InitializeTerminal);
        return new SaveData { instanceKey = UI_KEY };
    }

    private void InitializeTerminal(Terminal terminal, SaveData _)
    {
        _terminal = terminal;
        onPlayerSpawned.AddUniqueListener((player, _) => _terminal.UpdatePlayerPosition(player.transform.position));
        onPlayersPositionChanged.AddUniqueListener(playerTransform => _terminal.UpdatePlayerPosition(playerTransform.position));
    }

    public void SetNewData(SaveData newData) {}
    public SaveData GetCurrentData() => new() { instanceKey = UI_KEY };

    public void DestroyCurrentInstance() => Destroy(_terminal.gameObject);
}

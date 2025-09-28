using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "UISpawner", menuName = "SpawnersSO/UISpawner")]
public class UISpawnerSo : ScriptableObject, ISpawner
{
    [SerializeField] private Terminal uiPrefab;
    public void InitializeSpawner() => SpawnUi();

    private void SpawnUi()
    {
        var terminal = Instantiate(uiPrefab, Vector2.zero, Quaternion.identity);
        terminal.gameObject.name = "TERMINAL";
        OnTerminalSpawned?.Invoke(terminal, new SaveData{instanceKey = "TERMINAL"});
    }
}
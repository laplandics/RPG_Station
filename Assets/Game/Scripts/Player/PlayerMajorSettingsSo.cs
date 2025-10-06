using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsSo", menuName = "GameSettings/PlayerSettingsSo")]
public class PlayerMajorSettingsSo : ScriptableObject, IMajorSettings
{
    [SerializeField] private string playerKey;
    public string InstanceKey { get => playerKey; set => playerKey = value; }
    
    public int x;
    public int y;
    public int animationSpeed;
    public float baseStepsDelay;
    public float minStepsDelay;
    public float decreaseRate;

    public bool TrySet(IDataHandler handler) => handler is PlayerDataHandler && handler.TrySetSettings(this);
}
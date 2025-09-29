using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsSo", menuName = "GameSettings/PlayerSettingsSo")]
public class PlayerSettingsSo : ScriptableObject
{
    public string playerKey;
    
    public int x;
    public int y;
    public int animationSpeed;
    public float baseStepsDelay;
    public float minStepsDelay;
    public float decreaseRate;
}
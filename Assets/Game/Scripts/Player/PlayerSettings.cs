using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "GameSettings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public string playerKey;
    
    public int x;
    public int y;
}
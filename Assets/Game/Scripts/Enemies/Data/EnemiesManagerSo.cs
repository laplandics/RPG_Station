using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesManager", menuName = "ManagersSO/EnemiesManager")]
public class EnemiesManagerSo : ScriptableObject
{
    public const string ENEMY_KEY = "ENEMIES";

    public SaveData Initialize()
    {
        return new SaveData { instanceKey = ENEMY_KEY };
    }
}
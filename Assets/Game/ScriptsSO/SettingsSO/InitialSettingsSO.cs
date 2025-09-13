using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialSettings", menuName = "DataStorage/InitialSettings")]
public class InitialSettingsSO : ScriptableObject
{
    [Serializable]
    public class EnemyPrefabAmount
    {
        public GameObject enemyPrefab;
        public int amount;
    }
    
    public EnemyPrefabAmount[] Enemies;
}

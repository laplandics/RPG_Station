using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EnemiesSaveData
{
    public List<EnemyData> enemiesData = new();
    
    [Serializable]
    public class EnemyData
    {
        public string id;
        public bool isDead;
        public Vector3 position;
    }
}

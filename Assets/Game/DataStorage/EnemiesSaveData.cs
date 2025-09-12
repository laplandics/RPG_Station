using System;
using System.Collections.Generic;

[Serializable]
public class EnemiesSaveData
{
    [Serializable]
    public class EnemyData
    {
        public string id;
        public bool state;
    }

    public List<EnemyData> enemiesStates = new();


}

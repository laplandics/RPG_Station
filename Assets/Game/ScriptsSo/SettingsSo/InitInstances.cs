using UnityEngine;

[CreateAssetMenu(fileName = "InitialSettings", menuName = "DataStorage/InitialSettings")]
public class InitInstances : ScriptableObject
{
    public GameObject playerPrefab;
    public GameObject mapPrefab;
    public GameObject[] enemiesPrefabs;
}

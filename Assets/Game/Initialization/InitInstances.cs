using UnityEngine;

[CreateAssetMenu(fileName = "InitialSettings", menuName = "DataStorage/InitialSettings")]
public class InitInstances : ScriptableObject
{
    public Player playerPrefab;
    public Map mapPrefab;
    public Enemy[] enemiesPrefabs;
    public Chunk[] chunkPrefabs;
}

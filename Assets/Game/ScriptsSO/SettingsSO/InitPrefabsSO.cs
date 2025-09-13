using UnityEngine;

[CreateAssetMenu(fileName = "InitPrefabs", menuName = "DataStorage/InitPrefabs")]
public class InitPrefabsSO : ScriptableObject
{
    [Header("Game Objects")]
    public GameObject globalLightning;
    public GameObject camera;
    public GameObject map;
    public GameObject player;
}

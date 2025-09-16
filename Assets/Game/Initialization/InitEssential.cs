using UnityEngine;

[CreateAssetMenu(fileName = "InitPrefabs", menuName = "DataStorage/InitPrefabs")]
public class InitEssential : ScriptableObject
{
    [Header("Game Objects")]
    public GameObject globalLightning;
    public GameObject camera;
}

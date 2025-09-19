using UnityEngine;

[CreateAssetMenu(fileName = "MainGameObjectsManager", menuName = "DataStorage/MainGameObjectsManager")]
public class MainGameObjectsSo : ScriptableObject
{
    public GameObject[] allPrefabs;
}

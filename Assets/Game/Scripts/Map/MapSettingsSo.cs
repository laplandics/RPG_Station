using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "GameSettings/MapSettings")]
public class MapSettingsSo : ScriptableObject
{
    public string mapKey;
    
    public int chunkSize;
    public int mapSize;
    public int renderAreaSize;
    [Range(1000, 9999)] public int seed;
    [Range(0f, 0.1f)] public float scale;
    [Range(0.01f, 10f)] public float multX;
    [Range(0.01f, 10f)] public float multY;
    [Range(0, 10)] public int threshold;
}
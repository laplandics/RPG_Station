using UnityEngine;

[CreateAssetMenu(fileName = "MapDataStorage", menuName = "DataStorage/MapDataStorage")]
public class MapDataStorage : ScriptableObject
{
    public int chunkSize;
    public int mapSize;
    public int renderAreaSize;

    public Chunk[] terrainChunks;
    [Range(0.01f, 10f)] public float multX;
    [Range(1000, 9999)] public int seed;
    [Range(0.01f, 10f)] public float multY;
    [Range(0f, 0.1f)] public float scale;
    public int threshold;
}

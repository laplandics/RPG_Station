using UnityEngine;

public class WaterChunk : Chunk, ITerrainChunk
{
    [SerializeField] private float noiseMin;
    [SerializeField] private float noiseMax;
    [SerializeField] private TerrainType terrainType;
    public TerrainType TerrainType { get => terrainType; }

    public float NoiseMin => noiseMin;

    public float NoiseMax => noiseMax;
}

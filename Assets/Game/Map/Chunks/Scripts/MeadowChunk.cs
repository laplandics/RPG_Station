using UnityEngine;

public class MeadowChunk : Chunk, ITerrainChunk
{
    [SerializeField] private float noiseMin;
    [SerializeField] private float noiseMax;
    [SerializeField] private TerrainType terrainType;
    public TerrainType TerrainType { get => terrainType; }

    public float NoiseMin => noiseMin;

    public float NoiseMax => noiseMax;
}

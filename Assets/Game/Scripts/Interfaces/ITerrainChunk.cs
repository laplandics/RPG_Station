public enum TerrainType
{
    Water,
    Meadow,
    Forest,
    Mountaint
}

public interface ITerrainChunk
{
    public TerrainType TerrainType { get; }
    public float NoiseMin { get; }
    public float NoiseMax { get; }
}
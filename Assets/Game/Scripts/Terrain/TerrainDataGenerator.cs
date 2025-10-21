using UnityEngine;
using static MapHelper;
using static GameDataInjector;

[CreateAssetMenu(fileName = "TerrainDataGenerator", menuName = "GameSettings/Terrain/TerrainDataGenerator")]
public class TerrainDataGenerator : ScriptableObject
{
    public string terrainKey;
    public TerrainType terrainType;
    public int generationOrder;
    public float terrainWeight;
    public bool excludeFromTileSmoother;
    public TileSetSo tileSet;
    public ScriptableObject terrainGenerationPresets;
    public Material terrainMaterial;

    public Tile[] GenerateTerrainTilesInChunk(Vector2Int chunkIndex)
    {
        var chunkSize = InjectMapData.chunkSize;
        var tiles = new Tile[chunkSize * chunkSize];
        var tilePositions = GetTilesPositionsInChunk(chunkIndex);
        var preset = terrainGenerationPresets as IGenerationPreset;
        for (var i = 0; i < tilePositions.Count; i++)
        {
            var tilePosition = tilePositions[i];
            var tileNoise = preset!.GetNoise(tilePosition);
            if (!IsTileInBiome(tileNoise)) continue;
            var tile = new Tile();
            tile.Position = tilePosition;
            tile.TerrainType = terrainType;
            tiles[i] = tile;
        }
        return tiles;
    }

    private bool IsTileInBiome(float tileNoise)
    {
        return !(tileNoise < tileSet.noise.x) && !(tileNoise > tileSet.noise.y);
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static GameDataInjector;

public class ChunkCreator
{
    private GOService _goService;
    private Transform _map;
    private Chunk _chunkPrefab;
    private Material _tileMaterial;
    private TerrainDataGenerator[] _biomeGenerators;
    private int _chunkSize;
    
    private void AssignData(Transform map)
    {
        _goService = DS.GetSceneManager<GOService>();
        _map = map;
        _chunkSize = InjectMapData.chunkSize;
        _chunkPrefab = InjectMapSettings.chunkPrefab;
        _biomeGenerators = InjectTerrainTypesSettings.terrainDataGenerators;
    }

    private void GetTiles(Vector2Int chunkIndex, out Dictionary<TerrainType, Material> biomeMaterials, out Dictionary<Vector2Int, Tile> chunkTiles)
    {
        biomeMaterials = TileSetter.GetBiomeMaterials(_biomeGenerators);
        biomeMaterials.Add(InjectMapSettings.defaultTerrainType, InjectMapSettings.defaultMaterial);
        chunkTiles = TileSetter.GetTilesInChunk(chunkIndex, _biomeGenerators);
        TileSetter.FillEmptyTilesWithDefaultBiome(chunkTiles, chunkIndex);
        var excludedBiomes = TileSetter.GetExcludedFromTileSmootherBiomes(_biomeGenerators);
        TileSetter.ClearUncommonTilesInChunk(chunkTiles, excludedBiomes);
    }
    
    public Chunk CreateChunk(Vector2Int index, Transform map)
    {
        AssignData(map);
        GetTiles(index, out var biomes, out var tiles);
        var startTileX = index.x * _chunkSize;
        var startTileY = index.y * _chunkSize;
        var tileCount = _chunkSize * _chunkSize;
        var verts = new Vector3[tileCount * 4];
        var uvs = new Vector2[tileCount * 4];
        var biomeTriangles = new Dictionary<TerrainType, List<int>>();
        var usedBiomes = new List<TerrainType>();
        var v = 0;
        var textureScale = Grid.TileSize * 64;

        for (var y = 0; y < _chunkSize; y++)
        {
            for (var x = 0; x < _chunkSize; x++)
            {
                var globalTileX = startTileX + x;
                var globalTileY = startTileY + y;
                var key = new Vector2Int(globalTileX, globalTileY);
                var tile = tiles[key];
                var vertexPositionX = x * Grid.TileSize;
                var vertexPositionY = y * Grid.TileSize;
                var vertexIndex = v;
                verts[vertexIndex + 0] = new Vector3(vertexPositionX, vertexPositionY, 0f);
                verts[vertexIndex + 1] = new Vector3(vertexPositionX + Grid.TileSize, vertexPositionY, 0f);
                verts[vertexIndex + 2] = new Vector3(vertexPositionX, vertexPositionY + Grid.TileSize, 0f);
                verts[vertexIndex + 3] = new Vector3(vertexPositionX + Grid.TileSize, vertexPositionY + Grid.TileSize, 0f);
                float worldX = (startTileX + x) * Grid.TileSize;
                float worldY = (startTileY + y) * Grid.TileSize;
                var uvBL = new Vector2(worldX / textureScale, worldY / textureScale);
                var uvBR = new Vector2((worldX + Grid.TileSize) / textureScale, worldY / textureScale);
                var uvTL = new Vector2(worldX / textureScale, (worldY + Grid.TileSize) / textureScale);
                var uvTR = new Vector2((worldX + Grid.TileSize) / textureScale, (worldY + Grid.TileSize) / textureScale);
                uvs[vertexIndex + 0] = uvBL;
                uvs[vertexIndex + 1] = uvBR;
                uvs[vertexIndex + 2] = uvTL;
                uvs[vertexIndex + 3] = uvTR;
                var i0 = vertexIndex + 0;
                var i1 = vertexIndex + 1;
                var i2 = vertexIndex + 2;
                var i3 = vertexIndex + 3;
                var trisForTile = new[] { i0, i2, i1, i2, i3, i1 };
                var biome = tile.TerrainType;
                if (!biomeTriangles.TryGetValue(biome, out var list))
                {
                    list = new List<int>();
                    biomeTriangles[biome] = list;
                    usedBiomes.Add(biome);
                }
                list.AddRange(trisForTile);
                v += 4;
            }
        }

        return AssignMesh(index, verts, uvs, usedBiomes, biomeTriangles, biomes, startTileX, startTileY, tiles);
    }

    private Chunk AssignMesh(Vector2Int index, Vector3[] verts, Vector2[] uvs, List<TerrainType> usedBiomes, Dictionary<TerrainType, List<int>> biomeTriangles,
        Dictionary<TerrainType, Material> biomes, int startTileX, int startTileY, Dictionary<Vector2Int, Tile> tiles)
    {
        var mesh = new Mesh();
        if (verts.Length > 65000) mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.subMeshCount = usedBiomes.Count;
        for (var si = 0; si < usedBiomes.Count; si++)
        {
            var biome = usedBiomes[si];
            var trisList = biomeTriangles[biome];
            mesh.SetTriangles(trisList.ToArray(), si);
        }
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.MarkDynamic();
        var materials = new Material[usedBiomes.Count];
        for (var i = 0; i < usedBiomes.Count; i++)
        {
            var biome = usedBiomes[i];
            materials[i] = biomes[biome];
        }
        var chunk = SpawnChunk(mesh, startTileX, startTileY, index, tiles, materials);
        return chunk;
    }

    private Chunk SpawnChunk(Mesh mesh, int startTileX, int startTileY, Vector2Int position, Dictionary<Vector2Int, Tile> tiles, Material[] materials)
    {
        var chunkPosition = new Vector3(startTileX * Grid.TileSize, startTileY * Grid.TileSize, 1f);
        var chunk = _goService.Spawn(_chunkPrefab, chunkPosition, Quaternion.identity, _map);
        chunk.gameObject.name = $"Chunk {position.x}:{position.y}";
        chunk.Initialize(position, mesh, materials, tiles);
        return chunk;
    }
}
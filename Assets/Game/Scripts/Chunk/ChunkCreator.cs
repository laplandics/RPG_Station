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
    private int _textureScale;
    private int _transitionAtlasColumns;
    private int _transitionAtlasRows;
    
    private void AssignData(Transform map)
    {
        _goService = DS.GetSceneManager<GOService>();
        _map = map;
        _chunkSize = InjectMapData.chunkSize;
        _chunkPrefab = InjectMapSettings.chunkPrefab;
        _biomeGenerators = InjectTerrainTypesSettings.terrainDataGenerators;
        _transitionAtlasColumns = InjectMapData.columns;
        _transitionAtlasRows = InjectMapData.rows;
        _textureScale = Grid.TileSize * 64;
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
    
    public Chunk CreateChunk(Vector2Int chunkIndex, Transform map)
    {
        AssignData(map);
        GetTiles(chunkIndex, out var biomes, out var tiles);
        var startTileX = chunkIndex.x * _chunkSize;
        var startTileY = chunkIndex.y * _chunkSize;
        var tileCount = _chunkSize * _chunkSize;
        var verts = new Vector3[tileCount * 4];
        var uvs = new Vector2[tileCount * 4];
        var uvs2 = new Vector2[tileCount * 4];
        var biomeTriangles = new Dictionary<TerrainType, List<int>>();
        var usedTerrainTypes = new List<TerrainType>();
        var v = 0;

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
                GetUVs(worldX, worldY, vertexIndex, uvs);
                GetUVs2(0, vertexIndex, uvs2);
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
                    usedTerrainTypes.Add(biome);
                }
                list.AddRange(trisForTile);
                v += 4;
            }
        }

        return AssignMesh(chunkIndex, verts, uvs, usedTerrainTypes, biomeTriangles, biomes, startTileX, startTileY, tiles);
    }

    private void GetUVs(float worldX, float worldY, int vertexIndex, Vector2[] uvs)
    {
        var uvBL = new Vector2(worldX / _textureScale, worldY / _textureScale);
        var uvBR = new Vector2((worldX + Grid.TileSize) / _textureScale, worldY / _textureScale);
        var uvTL = new Vector2(worldX / _textureScale, (worldY + Grid.TileSize) / _textureScale);
        var uvTR = new Vector2((worldX + Grid.TileSize) / _textureScale, (worldY + Grid.TileSize) / _textureScale);
        uvs[vertexIndex + 0] = uvBL;
        uvs[vertexIndex + 1] = uvBR;
        uvs[vertexIndex + 2] = uvTL;
        uvs[vertexIndex + 3] = uvTR;
    }

    private void GetUVs2(int index, int vertexIndex, Vector2[] uvs)
    {
        var tileWidth = 1f / _transitionAtlasColumns;
        var tileHeight = 1f / _transitionAtlasRows;
        var tileX = index % _transitionAtlasColumns;
        var tileY = index / _transitionAtlasColumns;
        var uMin = tileX * tileWidth;
        var vMin = tileY * tileHeight;
        var uMax = uMin + tileWidth;
        var vMax = vMin + tileHeight;
        uvs[vertexIndex + 0] = new Vector2(uMin, vMin);
        uvs[vertexIndex + 1] = new Vector2(uMax, vMin);
        uvs[vertexIndex + 2] = new Vector2(uMin, vMax);
        uvs[vertexIndex + 3] = new Vector2(uMax, vMax);
    }

    private Chunk AssignMesh(Vector2Int index, Vector3[] verts, Vector2[] uvs, List<TerrainType> usedTerrainTypes, Dictionary<TerrainType, List<int>> biomeTriangles,
        Dictionary<TerrainType, Material> biomes, int startTileX, int startTileY, Dictionary<Vector2Int, Tile> tiles)
    {
        var mesh = new Mesh();
        if (verts.Length > 65000) mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.subMeshCount = usedTerrainTypes.Count;
        for (var si = 0; si < usedTerrainTypes.Count; si++)
        {
            var biome = usedTerrainTypes[si];
            var trisList = biomeTriangles[biome];
            mesh.SetTriangles(trisList.ToArray(), si);
        }
        var materials = new Material[usedTerrainTypes.Count];
        for (var i = 0; i < usedTerrainTypes.Count; i++)
        {
            var biome = usedTerrainTypes[i];
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
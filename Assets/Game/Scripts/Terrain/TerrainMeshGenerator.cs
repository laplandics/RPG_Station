using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator
{
    private Texture2D _atlas;
    private Material _tileMaterial;
    private int _mapSizeX; 
    private int _mapSizeY;
    private int _chunkSize;
    private int _atlasColumns;
    private int _atlasRows;
    private bool _isAtlasOriginTopLeft;
    private float _noiseScaleDividedBy100;
    private int _seed;

    private readonly Dictionary<Vector2Int, Chunk> _chunks = new();
    private const int TileSize = 1;
    private int[,] _tiles;

    // private void GenerateChunksMeshes()
    // {
    //     AssignMaterial();
    //     GenerateTilesData();
    //     ClearChunks();
    //     SplitMapInChunks();
    // }
    
    private void AssignMaterial()
    {
        if (_tileMaterial == null)
        {
            _tileMaterial = new Material(Shader.Find("Sprites/Default"))
            {
                mainTexture = _atlas,
                hideFlags = HideFlags.DontSave
            };
        }
        else { _tileMaterial.mainTexture = _atlas; }
    }
    
    
}
using System;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;
using static GameDataInjector;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    private Dictionary<Vector2Int, TileData> _tilesData = new();
    private RoutineService _routineService;
    private Vector2Int _chunkIndex;

    public void Initialize(Material tileMaterial, Mesh mesh, Vector2Int position, Dictionary<Vector2Int, int> tiles)
    {
        meshRenderer.sharedMaterial = tileMaterial;
        meshFilter.mesh = mesh;
        _chunkIndex = position;
        AssignTiles(tiles);
        SetUnreachableTiles();
        SendUnreachableTiles(Vector2Int.zero, DS.GetSceneManager<SceneInitializeService>().PlayerInitializer.PlayerController);
        OnSmbEnteredChunk.AddListener(SendUnreachableTiles);
    }

    private void AssignTiles(Dictionary<Vector2Int, int> tiles)
    {
        foreach (var tile in tiles)
        {
            foreach (var tileData in InjectTilesData.allTilesData)
            {
                if (tile.Value != tileData.tileAtlasIndex) continue;
                _tilesData.Add(tile.Key, tileData);
            }
        }
    }

    private void SetUnreachableTiles()
    {
        
    }
    private void SendUnreachableTiles(Vector2Int chunk, IWalkable smb)
    {
        
    }
}
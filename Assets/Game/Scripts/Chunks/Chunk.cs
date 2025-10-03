using System;
using System.Collections.Generic;
using UnityEngine;
using static BiomeTypePairs;
using static EventManager;
using static GlobalMapMethods;
using static MapDataHandler;

public class Chunk : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    private readonly Dictionary<Vector2Int, BiomeType> _tileBiomePairs = new();
    private readonly HashSet<Vector2Int> _unreachableTiles = new();
    private RoutineService _routineService;
    private Vector2Int _chunkIndex;

    public void Initialize(Material tileMaterial, Mesh mesh, Vector2Int position, Dictionary<Vector2Int, int> tiles)
    {
        meshRenderer.sharedMaterial = tileMaterial;
        meshFilter.mesh = mesh;
        _chunkIndex = position;
        AssignTileBiomes(tiles);
        SetUnreachableTiles();
        SendUnreachableTiles(Vector2Int.zero, DS.GetSceneManager<SceneInitializeService>().PlayerInitializer.PlayerController);
        OnSmbEnteredChunk.AddListener(SendUnreachableTiles);
    }

    private void AssignTileBiomes(Dictionary<Vector2Int, int> tiles)
    {
        foreach (var tile in tiles)
        {
            BiomeType biomeType; 
            if (BiomeTypes.TryGetValue(tile.Value, out var biome)) biomeType  = biome;
            else throw new ArgumentOutOfRangeException($"No biome for index {tile.Value}");
            _tileBiomePairs.TryAdd(tile.Key, biomeType);
        }
    }

    private void SetUnreachableTiles()
    {
        foreach (var tileBiomePair in _tileBiomePairs)
        {
            if (tileBiomePair.Value is BiomeType.Water or BiomeType.Mountain) _unreachableTiles.Add(tileBiomePair.Key);
        }
    }
    private void SendUnreachableTiles(Vector2Int chunk, IWalkable smb)
    {
        if (!GetChunksIndexesInArea(GetMapData.tilesCalculationArea, chunk).Contains(_chunkIndex)) return;
        foreach (var unreachableTile in _unreachableTiles) { smb.UnreachableTiles.Add(unreachableTile); }
    }
}
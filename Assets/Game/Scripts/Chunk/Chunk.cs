using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    public Vector2Int chunkIndex;
    [NonSerialized] public Mesh Mesh;
    [NonSerialized] public Dictionary<Vector2Int, Tile> TilesData = new();
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    
    public void Initialize(Vector2Int position, Mesh newMesh, Material[] tileMaterials, Dictionary<Vector2Int, Tile> tiles)
    {
        chunkIndex = position;
        Mesh = newMesh;
        meshFilter.mesh = newMesh;
        meshRenderer.sharedMaterials = tileMaterials;
        TilesData = tiles;
    }
}

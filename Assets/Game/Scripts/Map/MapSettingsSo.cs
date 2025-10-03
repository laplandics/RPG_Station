using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "GameSettings/MapSettings")]
public class MapSettingsSo : ScriptableObject
{
    [Header("SaveLoad Settings")]
    public string mapKey;
    
    [Header("MapSize Settings")]
    public int chunkSize;
    public int renderChunksCount;
    [Range(0, 10000)] public int memorizedArea;
    [Range(0, 10)] public int tilesCalculationArea;
    
    [Header("NoiseGeneration Settings")]
    [Range(10000, 999999)] public int seed;
    [Range(0f, 0.1f)] public float scale;
    [Range(1, 20)] public int octaves;
    [Range(0.0f, 2.0f)] public float persistence;
    [Range(0.0f, 10.0f)] public float lacunarity;

    [Header("MeshGeneration Settings")]
    public Chunk chunkPrefab;
    public Texture2D atlasTexture;
    public int atlasColumns;
    public int atlasRows;
    
    [NonSerialized] public Queue<Vector2Int> MemorizedChunks =  new();
}
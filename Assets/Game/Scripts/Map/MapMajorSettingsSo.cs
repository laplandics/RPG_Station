using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "GameSettings/MapSettings")]
public class MapMajorSettingsSo : ScriptableObject, IMajorSettings
{
    [Header("SaveLoad Settings")]
    [SerializeField] private string mapKey;
    public string InstanceKey { get => mapKey; set => mapKey = value; }

    [Header("MapSize Settings")]
    public int chunkSize;
    public int renderChunksCount;
    [Range(0, 10000)] public int memorizedArea;
    [Range(3, 10)] public int tilesCalculationArea;

    [Header("TypeMap Settings")]
    public int visibleChunks;
    
    [Header("Generation Settings")]
    [Range(100000, 999999)]public uint seed;
    public Chunk chunkPrefab;
    public TerrainType defaultTerrainType;
    public Material defaultMaterial;
    
    [NonSerialized] public Queue<Vector2Int> MemorizedChunks =  new();
    
    public bool TrySet(IDataHandler handler) => handler is MapDataHandler && handler.TrySetSettings(this);
}
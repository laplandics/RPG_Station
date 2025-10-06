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
    
    [Header("MeshGeneration Settings")]
    public Chunk chunkPrefab;
    public Texture2D atlasTexture;
    public int atlasColumns;
    public int atlasRows;
    
    [NonSerialized] public Queue<Vector2Int> MemorizedChunks =  new();
    
    public bool TrySet(IDataHandler handler) => handler is MapDataHandler && handler.TrySetSettings(this);
}
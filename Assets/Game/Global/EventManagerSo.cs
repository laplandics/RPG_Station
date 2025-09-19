using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "EventManager", menuName = "ManagersSO/EventManager")]
public class EventManagerSo : ScriptableObject
{
    public UnityEvent onInSceneManagersInitialized = new();
    public UnityEvent onManagersInitialized = new();
    public UnityEvent<Light2D, Camera> onUnityEssentialsSpawned = new();
    public UnityEvent<Player> onPlayerSpawned = new();
    public UnityEvent<Map> onMapSpawned = new();
    public UnityEvent onSceneReady = new();
    public UnityEvent onSave = new();
    public UnityEvent onLoad = new();
    public UnityEvent onMapUpdated;
    public UnityEvent onPlayersPositionChanged = new();
    public UnityEvent<Chunk> onChunkSpawned = new();
    public UnityEvent<Chunk> onChunkDespawned = new();
}
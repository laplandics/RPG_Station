using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "EventManager", menuName = "ManagersSO/EventManager")]
public class EventManagerSo : ScriptableObject
{
    [NonSerialized] public UnityEvent onInSceneManagersInitialized = new();
    [NonSerialized] public UnityEvent onManagersInitialized = new();
    [NonSerialized] public UnityEvent<Light2D, Camera> onUnityEssentialsSpawned = new();
    [NonSerialized] public UnityEvent<Player> onPlayerSpawned = new();
    [NonSerialized] public UnityEvent<Map> onMapSpawned = new();
    [NonSerialized] public UnityEvent<Terminal> onTerminalSpawned = new();
    [NonSerialized] public UnityEvent onSceneReady = new();
    [NonSerialized] public UnityEvent onSave = new();
    [NonSerialized] public UnityEvent onLoad = new();
    [NonSerialized] public UnityEvent onMapUpdated = new();
    [NonSerialized] public UnityEvent<Transform> onPlayersPositionChanged = new();
    [NonSerialized] public UnityEvent<Chunk> onChunkSpawned = new();
    [NonSerialized] public UnityEvent<Chunk> onChunkDespawned = new();
    [NonSerialized] public UnityEvent<Enemy> onEnemySpawned = new();
    [NonSerialized] public UnityEvent<Enemy> onEnemyDespawned = new();
    [NonSerialized] public UnityEvent<float> onTimePassed = new();
}
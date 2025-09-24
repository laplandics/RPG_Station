using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "EventManager", menuName = "ManagersSO/EventManager")]
public class EventManagerSo : ScriptableObject
{
    //MANAGERS//
    [NonSerialized] public UnityEvent onInSceneManagersInitialized = new();
    [NonSerialized] public UnityEvent onManagersInitialized = new();

    //INITIAL_INSTANCES//
    [NonSerialized] public UnityEvent<Light2D, Camera> onUnityEssentialsSpawned = new();
    [NonSerialized] public UnityEvent<Player> onPlayerSpawned = new();
    [NonSerialized] public UnityEvent<Map> onMapSpawned = new();
    [NonSerialized] public UnityEvent<Terminal> onTerminalSpawned = new();

    //GLOBAL_EVENTS//
    [NonSerialized] public UnityEvent onSceneReady = new();
    [NonSerialized] public UnityEvent onSave = new();
    [NonSerialized] public UnityEvent onLoad = new();
    [NonSerialized] public UnityEvent<float> onTimePassed = new();

    //MAP_EVENTS//
    [NonSerialized] public UnityEvent<Map> onMapGenerated = new();

    //PLAYER_EVENTS//
    [NonSerialized] public UnityEvent<Transform> onPlayersPositionChanged = new();

    //CHUNKS_EVENTS//
    [NonSerialized] public UnityEvent<Vector2Int, Chunk> onChunkSpawned = new();
    [NonSerialized] public UnityEvent<Vector2Int, Chunk, bool> onChunkDespawned = new();

    //ENEMIES_EVENTS//
    [NonSerialized] public UnityEvent<Chunk, Enemy> onEnemySpawned = new();
    [NonSerialized] public UnityEvent<Chunk, Enemy> onEnemyDespawned = new();
}
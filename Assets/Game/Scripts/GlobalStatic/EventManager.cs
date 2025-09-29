using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public static class EventManager
{
    //MANAGERS//
    public static readonly UnityEvent OnInSceneManagersInitialized = new();
    
    //GLOBAL_EVENTS//
    public static readonly UnityEvent OnSceneReady = new();
    public static readonly UnityEvent<float> OnTimePassed = new();

    //CAMERA_EVENTS//
    public static readonly UnityEvent<Camera, SaveData> OnCameraSpawned = new();

    //LIGHT_EVENTS//
    public static readonly UnityEvent<Light2D, SaveData> OnLightSpawned = new();

    //SAVE_LOAD_EVENTS//
    public static readonly UnityEvent OnSave = new();
    public static readonly UnityEvent OnLoad = new();

    //UI_EVENTS//
    public static readonly UnityEvent<Terminal, SaveData> OnTerminalSpawned = new();

    //MAP_EVENTS//
    public static readonly UnityEvent<Map, MapData, TerrainsData> OnMapSpawned = new();
    public static readonly UnityEvent<Vector2Int> OnRenderAreaBorderReached = new();

    //PLAYER_EVENTS//
    public static readonly UnityEvent<Player, PlayerData> OnPlayerSpawned = new();
    public static readonly UnityEvent<Transform> OnPlayersPositionChanged = new();
    public static readonly UnityEvent<Vector2Int> OnPlayerChunkChanged = new();

}
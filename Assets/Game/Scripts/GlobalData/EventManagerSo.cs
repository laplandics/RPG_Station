using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public static class EventManager
{
    //MANAGERS//
    static public UnityEvent onInSceneManagersInitialized = new();

    //CAMERA_EVENTS//
    static public UnityEvent<Camera, SaveData> onCameraSpawned = new();

    //LIGHT_EVENTS//
    static public UnityEvent<Light2D, SaveData> onLightSpawned = new();

    //GLOBAL_EVENTS//
    static public UnityEvent onSceneReady = new();
    static public UnityEvent<float> onTimePassed = new();

    //SAVE_LOAD_EVENTS//
    static public UnityEvent onSave = new();
    static public UnityEvent onLoad = new();

    //UI_EVENTS//
    static public UnityEvent<Terminal, SaveData> onTerminalSpawned = new();

    //MAP_EVENTS//
    static public UnityEvent<Map, SaveData> onMapSpawned = new();
    static public UnityEvent<Vector2> onRenderAreaBorderReached = new();

    //PLAYER_EVENTS//
    static public UnityEvent<Player, SaveData> onPlayerSpawned = new();
    static public UnityEvent<Transform> onPlayersPositionChanged = new();

    public static void AddUniqueListener(this UnityEvent unityEvent, UnityAction call)
    {
        unityEvent.RemoveListener(call);
        unityEvent.AddListener(call);
    }

    public static void AddUniqueListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> call)
    {
        unityEvent.RemoveListener(call);
        unityEvent.AddListener(call);
    }

    public static void AddUniqueListener<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call)
    {
        unityEvent.RemoveListener(call);
        unityEvent.AddListener(call);
    }
}
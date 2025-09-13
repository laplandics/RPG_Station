using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EventManager", menuName = "ManagersSO/EventManager")]
public class EventManagerSo : ScriptableObject
{
    public UnityEvent onInSceneManagersInitialized = new();
    public UnityEvent onSceneInitializationCompleted = new();
    public UnityEvent onSave = new();
    public UnityEvent onLoad = new();
    
}
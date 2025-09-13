using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameManager", menuName = "ManagersSO/GameManager")]
public class GameManagerSO : ScriptableObject
{
    public UnityEvent OnSave = new();
    public UnityEvent OnLoad =  new();
}

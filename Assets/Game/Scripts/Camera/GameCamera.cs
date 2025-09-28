using Unity.Cinemachine;
using UnityEngine;
using static EventManager;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cameraCm;

    public void Initialize()
    {
        OnPlayerSpawned.AddListener(SetTarget);
    }

    private void SetTarget(Player target, SaveData _)
    {
        cameraCm.Follow = target.transform;
    }
}

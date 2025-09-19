using Unity.Cinemachine;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cameraCm;

    public void Initialize()
    {
        DS.GetSoManager<EventManagerSo>().onPlayerSpawned.AddListener(SetTarget);
    }

    public void SetTarget(Player target)
    {
        cameraCm.Follow = target.transform;
    }
}

using Unity.Cinemachine;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cameraCm;
    public void SetTarget(Transform target)
    {
        cameraCm.Follow = target;
    }
}

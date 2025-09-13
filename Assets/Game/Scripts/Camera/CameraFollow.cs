using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private new CinemachineCamera camera;

    public void SetTarget(Transform target)
    {
        camera.Follow = target;
    }
}

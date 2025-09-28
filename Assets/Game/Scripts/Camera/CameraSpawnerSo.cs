using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "CameraSpawner", menuName = "SpawnersSO/CameraSpawner")]
public class CameraSpawnerSo : ScriptableObject, ISpawner
{
    [SerializeField] private GameObject cameraPrefab;

    public void InitializeSpawner() => SpawnCamera();

    private void SpawnCamera()
    {
        var camera = Instantiate(cameraPrefab, Vector2.zero, Quaternion.identity);
        camera.name = "CAMERA";
        var gameCamera = camera.GetComponentInChildren<Camera>();
        OnCameraSpawned?.Invoke(gameCamera, new SaveData{instanceKey = "CAMERA"});
    }
}
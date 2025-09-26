using System.Collections;
using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "CameraSpawner", menuName = "SpawnersSO/CameraSpawner")]
public class CameraSpawnManagerSo : ScriptableObject, ISpawner
{
    [SerializeField] private GameObject _cameraPrefab;
    private SaveData _cameraData;

    public bool TryInitializeSpawner(SaveData data)
    {
        if (data.instanceKey != GameCameraManagerSo.CAMERA_KEY) return false;
        _cameraData = data;
        DS.GetSceneManager<RoutineManager>().StartRoutine(SpawnCamera());
        return true;
    }

    private IEnumerator SpawnCamera()
    {
        var camera = Instantiate(_cameraPrefab, Vector2.zero, Quaternion.identity);
        camera.name = _cameraData.instanceKey;
        var gameCamera = camera.GetComponentInChildren<Camera>();
        onCameraSpawned?.Invoke(gameCamera, _cameraData);
        yield break;
    }
}
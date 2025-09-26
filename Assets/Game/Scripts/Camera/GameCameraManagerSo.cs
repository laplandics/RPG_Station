using UnityEngine;
using static EventManager;

[CreateAssetMenu(fileName = "CameraManager", menuName = "ManagersSO/CameraManager")]
public class GameCameraManagerSo : ScriptableObject, IObjectManagerSo
{
    public const string CAMERA_KEY = "CAMERA";
    private Camera _camera;
    private GameCamera _gameCamera;

    public string Key => CAMERA_KEY;

    public SaveData Initialize()
    {
        onCameraSpawned.AddUniqueListener(SetCamera);
        return new SaveData { instanceKey = CAMERA_KEY };
    }

    private void SetCamera(Camera camera, SaveData _)
    {
        _camera = camera;
        _gameCamera = _camera.GetComponentInParent<GameCamera>();
        _gameCamera.Initialize();
    }
    
    public void SetNewData(SaveData _) { }

    public SaveData GetCurrentData() => new() { instanceKey = CAMERA_KEY };

    public void DestroyCurrentInstance() => Destroy(_gameCamera.gameObject);
}
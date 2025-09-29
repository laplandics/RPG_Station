using UnityEngine;
using static EventManager;

public class CameraInitializer
{
    private Camera _camera;
    private GameCamera _gameCamera;
    private readonly GOService _goService;

    public CameraInitializer()
    {
        OnCameraSpawned.AddListener(SetCamera);
        _goService = DS.GetSceneManager<GOService>();
    }

    private void SetCamera(Camera camera, SaveData _)
    {
        _camera = camera;
        _gameCamera = _camera.GetComponentInParent<GameCamera>();
        _gameCamera.Initialize();
    }

    public void DeInitialize()
    {
        _goService.Despawn(_gameCamera.gameObject);
        OnCameraSpawned.RemoveListener(SetCamera);
    }
}
using UnityEngine;
using static EventManager;

public class CameraInitializer
{
    private Camera _camera;
    private GameCamera _gameCamera;
    private readonly GameObjectsService _gameObjectsService;

    public CameraInitializer()
    {
        OnCameraSpawned.AddUniqueListener(SetCamera);
        _gameObjectsService = DS.GetSceneManager<GameObjectsService>();
    }

    private void SetCamera(Camera camera, SaveData _)
    {
        _camera = camera;
        _gameCamera = _camera.GetComponentInParent<GameCamera>();
        _gameCamera.Initialize();
    }

    public void DeInitialize() => _gameObjectsService.Despawn(_gameCamera.gameObject);
}
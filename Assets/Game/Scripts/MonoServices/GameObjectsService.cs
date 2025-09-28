using UnityEngine;

public class GameObjectsService : MonoBehaviour, IInSceneService
{
    public void Initialize() {}

    public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
    {
        return parent ? Instantiate(prefab, position, rotation, parent) : Instantiate(prefab, position, rotation);
    }

    public void Despawn(GameObject obj) => Destroy(obj);
}

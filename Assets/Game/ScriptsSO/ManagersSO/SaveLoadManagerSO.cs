using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoadManager", menuName = "ManagersSO/SaveLoadManager")]
public class SaveLoadManagerSO : ScriptableObject, IRoutineManagerSo
{
    private Coroutine waitForKeyToSaveLoadRoutine;
    
    public void OnRoutineAvailable()
    {
        DS.GetInSceneManager<RoutineManager>().GetUpdateAction(WaitForKeyToSaveLoad);
    }

    public void OnRoutineUnavailable()
    {
        DS.GetInSceneManager<RoutineManager>().ReturnUpdateAction(WaitForKeyToSaveLoad);
    }

    private void WaitForKeyToSaveLoad()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            DS.GetManager<SaveLoadSystemManagerSO>().Save();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            DS.GetManager<SaveLoadSystemManagerSO>().Load(out var enemiesData, out var playerData);
            var player = FindFirstObjectByType<Player>();
            player.transform.position = playerData.position;
            player.SpriteRenderer.sprite = playerData.sprite;
        }
    }
}

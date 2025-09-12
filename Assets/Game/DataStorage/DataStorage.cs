using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public SaveDataSO saveData;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

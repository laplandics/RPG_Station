using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Action OnAllPersistentDataSaved { get; set; }
    public Action OnAllPersistentDataLoaded { get; set; }

    private void Awake()
    {
        if (!Instance) {Instance = this; DontDestroyOnLoad(gameObject);}
        else {Destroy(gameObject);}
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F5))
        {
            SaveloadSystem.SaveAll();
        }

        if (Input.GetKey(KeyCode.F9))
        {
            SaveloadSystem.LoadAll();
        }
    }
}
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
}
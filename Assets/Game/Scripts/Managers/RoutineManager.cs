using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineManager : MonoBehaviour, IInitializable
{
    [SerializeField] private int initializeOrder;
    public int InitializeOrder => initializeOrder;
    private List<Action> actions = new();
    
    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    public Coroutine GetCoroutine(IEnumerator routine)
    {
        var newCoroutine = StartCoroutine(routine);
        return newCoroutine;
    }

    public void ReturnCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }

    public void GetUpdateAction(Action action) => actions.Add(action);
    
    public void ReturnUpdateAction(Action action) => actions.Remove(action);
    

    private void Update()
    {
        foreach (var action in actions)
        {
            action?.Invoke();
        }
    }

}

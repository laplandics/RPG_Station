using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineManager : MonoBehaviour, IInSceneManager
{
    [SerializeField] private int initializeOrder;
    public int InitializeOrder => initializeOrder;
    private readonly List<Action> _updateActions = new();
    private readonly List<Action> _fixedUpdateActions = new();

    public void Initialize() {}
    public Coroutine StartRoutine(IEnumerator routine) {var newRoutine = StartCoroutine(routine); return newRoutine;}
    public void EndRoutine(Coroutine coroutine) {StopCoroutine(coroutine);}
    public void GetUpdateAction(Action action) => _updateActions.Add(action);
    public void GetFixedUpdateAction(Action action) => _fixedUpdateActions.Add(action);
    public void ReturnUpdateAction(Action action) => _updateActions.Remove(action);
    public void ReturnFixedUpdateAction(Action action) => _fixedUpdateActions.Remove(action);
    private void Update() {foreach (var action in _updateActions){action?.Invoke();}}
    private void FixedUpdate(){foreach (var action in _fixedUpdateActions){action?.Invoke();}}

}

using System;
using UnityEngine;

public enum GlobalInputsState
{
    AllEnable,
    SaveLoadBlocked
}

[CreateAssetMenu(fileName = "GameInputsManager", menuName = "ManagersSO/GameInputsManager")]
public class GlobalInputsManagerSo : ScriptableObject
{
    private GameInputs _inputs;

    private void OnEnable()
    {
        _inputs = new GameInputs();
    }

    public GameInputs GetInputs() => _inputs;
    public void BlockGlobalInputs()
    {
        _inputs.Global.Disable();
    }
    public void AssignInputAction(GlobalInputsState state)
    {
        _inputs.Global.Enable();
        
        switch (state)
        {
            case GlobalInputsState.AllEnable:
                _inputs.Global.Save.performed += _ => DS.GetSoManager<EventManagerSo>().onSave?.Invoke();
                _inputs.Global.Load.performed += _ => DS.GetSoManager<EventManagerSo>().onLoad?.Invoke();
                break;
            case GlobalInputsState.SaveLoadBlocked:
                _inputs.Global.Save.Disable();
                _inputs.Global.Load.Disable();
                break;
        }
        
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using static EventManager;

[CreateAssetMenu(fileName = "GameInputsManager", menuName = "ManagersSO/GameInputsManager")]
public class GlobalInputsManagerSo : ScriptableObject
{
    private GameInputs _inputs;

    private void OnEnable()
    {
        _inputs = new GameInputs();
        _inputs.Global.Disable();
    }

    public GameInputs GetInputs() => _inputs;

    public void EnableAllGlobalInputs()
    {
        _inputs.Global.Enable();
        EnableSaveLoadInputs();
    }
    
    public void DisableAllGlobalInputs()
    {
        _inputs.Global.Disable();
        DisableSaveLoadInputs();
    }
    private void EnableSaveLoadInputs()
    {
        _inputs.Global.Save.performed += InvokeSave;
        _inputs.Global.Load.performed += InvokeLoad;
    }

    private void DisableSaveLoadInputs()
    {
        _inputs.Global.Save.performed -= InvokeSave;
        _inputs.Global.Load.performed -= InvokeLoad;
    }


    private void InvokeSave(InputAction.CallbackContext _)
    {
        OnSave?.Invoke();
    }
    private void InvokeLoad(InputAction.CallbackContext _)
    {
        OnLoad?.Invoke();
    }
}
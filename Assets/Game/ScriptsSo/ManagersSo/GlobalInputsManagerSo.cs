using UnityEngine;
using UnityEngine.InputSystem;

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
    public void EnableSaveLoadInputs()
    {
        _inputs.Global.Save.performed += InvokeSave;
        _inputs.Global.Load.performed += InvokeLoad;
    }

    public void DisableSaveLoadInputs()
    {
        _inputs.Global.Save.performed -= InvokeSave;
        _inputs.Global.Load.performed -= InvokeLoad;
    }

    private void InvokeSave(InputAction.CallbackContext _)
    {
        DS.GetSoManager<EventManagerSo>().onSave?.Invoke();
    }
    private void InvokeLoad(InputAction.CallbackContext _)
    {
        DS.GetSoManager<EventManagerSo>().onLoad?.Invoke();
    }
}
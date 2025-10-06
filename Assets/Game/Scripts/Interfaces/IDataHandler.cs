using UnityEngine;

public interface IDataHandler
{
    public T GetData<T>() where T : SaveData;
    public T GetSettings<T>() where T : ScriptableObject;
    public bool TrySetSettings(IMajorSettings majorSettings);
    public SaveData SendReceiveData(SaveData saveData);
    public bool TryLoadData();
    
    public bool TrySaveData();
}
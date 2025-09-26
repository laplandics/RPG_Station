using UnityEngine;

public interface IObjectManagerSo
{
    public string Key { get; }
    public SaveData Initialize();
    public SaveData GetCurrentData();
    public void SetNewData(SaveData newData);
    public void DestroyCurrentInstance();
}
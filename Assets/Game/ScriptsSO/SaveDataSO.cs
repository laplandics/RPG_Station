using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "SaveDataSO/SaveData")]

public class SaveDataSO : ScriptableObject
{
    private readonly PlayerSaveData playerSaveData =  new();
    private readonly EnemiesSaveData enemiesSaveData = new();
    
    [Serializable]
    public struct SaveData
    {
        public PlayerSaveData playerSaveData;
        public EnemiesSaveData enemiesSaveData;
    }
    public SaveData saveData;

    private void OnEnable()
    {
        saveData.playerSaveData = playerSaveData;
        saveData.enemiesSaveData = enemiesSaveData;
        SaveloadSystem.SaveDataSo = this;
    }
}
using UnityEngine;

public class PlayerDataHandler : IDataHandler
{
    private PlayerData _playerData;
    private PlayerMajorSettingsSo _playerSettings;

    public T GetData<T>() where T : SaveData => _playerData as T;
    public T GetSettings<T>() where T : ScriptableObject  => _playerSettings as T;

    public bool TrySetSettings(IMajorSettings majorSettings)
    {
        if (majorSettings is not PlayerMajorSettingsSo ps) return false;
        _playerSettings = ps;
        _playerData = new PlayerData
        {
            instanceKey = ps.InstanceKey,
            x = ps.x,
            y = ps.y,
            animationSpeed = ps.animationSpeed,
            baseStepsDelay = ps.baseStepsDelay,
            minStepsDelay = ps.minStepsDelay,
            decreaseRate = ps.decreaseRate
        };
        return true;
    }

    public SaveData SendReceiveData(SaveData data = null)
    {
        var playerData = data as PlayerData;
        playerData ??= DS.GetSceneManager<SceneInitializeService>().PlayerInitializer.CurrentPlayerData;
        _playerData = playerData;
        
        _playerSettings.InstanceKey = playerData.instanceKey;
        _playerSettings.x = playerData.x;
        _playerSettings.y = playerData.y;

        return _playerData;
    }

    public bool TryLoadData()
    {
        var data = DS.GetGlobalManager<SaveLoadManagerSo>().Load<PlayerData>(_playerSettings.InstanceKey);
        return data != null && SendReceiveData(data) is PlayerData;
    }

    public bool TrySaveData()
    {
        var data = SendReceiveData();
        if (data is not PlayerData playerData) return false;
        DS.GetGlobalManager<SaveLoadManagerSo>().Save(playerData.instanceKey, playerData);
        return true;
    }
}
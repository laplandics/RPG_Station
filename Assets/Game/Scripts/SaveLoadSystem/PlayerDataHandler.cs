public static class PlayerDataHandler
{
    public static PlayerData GetPlayerData {get; private set;}
    public static PlayerSettingsSo GetPlayerSettingsSo {get; private set;}
    
    public static void SetPlayerSettings(PlayerSettingsSo ps)
    {
        GetPlayerSettingsSo = ps;
        GetPlayerData = new PlayerData
        {
            instanceKey = ps.playerKey,
            x = ps.x,
            y = ps.y,
            animationSpeed = ps.animationSpeed,
            baseStepsDelay = ps.baseStepsDelay,
            minStepsDelay = ps.minStepsDelay,
            decreaseRate = ps.decreaseRate
      };
    }

    public static PlayerData SetPlayerData(PlayerData playerData = null)
    {
        playerData ??= DS.GetSceneManager<SceneInitializeService>().PlayerInitializer.CurrentPlayerData;
        GetPlayerData = playerData;
        
        GetPlayerSettingsSo.playerKey = playerData.instanceKey;
        GetPlayerSettingsSo.x = playerData.x;
        GetPlayerSettingsSo.y = playerData.y;

        return GetPlayerData;
    }
}
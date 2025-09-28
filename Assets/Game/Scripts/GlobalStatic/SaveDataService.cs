using System.Collections.Generic;
public static class SaveDataService
{
    public static MapData GetMapData { get; private set; }
    
    public static void SaveMapSettings(MapSettingsSo ms)
    {
        GetMapData = new MapData
        {
            instanceKey = ms.mapKey,
            chunkSize = ms.chunkSize,
            mapSize = ms.mapSize,
            renderAreaSize = ms.renderAreaSize,
            seed = ms.seed,
            scale = ms.scale,
            multX = ms.multX,
            multY = ms.multY,
            threshold = ms.threshold
        };
    }

    public static PlayerData GetPlayerData {get; private set;}
    
    public static void SavePlayerData(PlayerSettings ps)
    {
        GetPlayerData = new PlayerData
        {
            instanceKey = ps.playerKey,
            x = ps.x,
            y = ps.y
        };
    }
    
    private static TerrainsData GetTerrainsData {get; set;}
    
    public static void SaveTerrainSettings(TerrainSettingsSo[] terrainSettings)
    {
        GetTerrainsData = new TerrainsData { AllTerrainsData = new Dictionary<TerrainType, TerrainData>() };
        foreach (var ts in terrainSettings)
        {
            var prefabKeys = new List<string>();
            foreach (var enemy in ts.allowedEnemies) { prefabKeys.Add(enemy.prefabKey); }
            
            GetTerrainsData.AllTerrainsData.TryAdd
            (
                ts.terrainType,
                new TerrainData
                {
                    instanceKey = ts.terrainKey,
                    terrainType = ts.terrainType,
                    noiseMax = ts.noiseMax,
                    noiseMin = ts.noiseMin,
                    enemySpawnMult = ts.enemySpawnMult,
                    allowedEnemiesPrefabKeys = prefabKeys
                }
            );
        }
    }
}
using System.Collections.Generic;

public static class TerrainDataHandler
{
    public static TerrainSettingsSo[] GetTerrainSettingsSo {get; private set;}
    public static AllTerrainsData GetAllTerrainsData {get; private set;}
    
    public static void SetTerrainSettings(TerrainSettingsSo[] terrainSettings)
    {
        GetTerrainSettingsSo = terrainSettings;
        GetAllTerrainsData = new AllTerrainsData { instanceKey = "TERRAIN", allTerrainsData = new List<TerrainData>() };
        foreach (var ts in terrainSettings)
        {
            var prefabKeys = new List<string>();
            foreach (var enemy in ts.allowedEnemies) { prefabKeys.Add(enemy.prefabKey); }
            GetAllTerrainsData.allTerrainsData.Add
            (
                new TerrainData
                {
                    instanceKey = ts.terrainKey,
                    biomeType = ts.biomeType,
                    noise = ts.noise,
                    enemySpawnMult = ts.enemySpawnMult,
                    allowedEnemiesPrefabKeys = prefabKeys
                }
            );
        }
    }

    public static AllTerrainsData SetTerrainsData(AllTerrainsData allTerrainsData = null)
    {
        allTerrainsData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentAllTerrainsData;
        GetAllTerrainsData = allTerrainsData;

        var allEnemies = new Dictionary<string, Enemy>();
        foreach (var settings in GetTerrainSettingsSo)
        {
            foreach (var enemy in settings.allowedEnemies)
            {
                allEnemies.TryAdd(enemy.prefabKey, enemy);
            }
        }
        foreach (var terrainSetting in GetTerrainSettingsSo)
        {
            foreach (var terrainData in GetAllTerrainsData.allTerrainsData)
            {
                if (terrainSetting.biomeType != terrainData.biomeType) continue;
                terrainSetting.noise = terrainData.noise;
                terrainSetting.enemySpawnMult = terrainData.enemySpawnMult;
                terrainSetting.allowedEnemies.Clear();
                foreach (var key in terrainData.allowedEnemiesPrefabKeys)
                {
                    terrainSetting.allowedEnemies.Add(allEnemies[key]);
                }
            }
        }

        return GetAllTerrainsData;
    }
}
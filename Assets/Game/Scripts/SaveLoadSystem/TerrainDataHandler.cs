using System.Collections.Generic;

public static class TerrainDataHandler
{
    public static TerrainSettingsSo[] GetTerrainSettingsSo {get; private set;}
    public static TerrainsData GetTerrainsData {get; private set;}
    
    public static void SetTerrainSettings(TerrainSettingsSo[] terrainSettings)
    {
        GetTerrainSettingsSo = terrainSettings;
        GetTerrainsData = new TerrainsData { instanceKey = "TERRAIN", allTerrainsData = new List<TerrainData>() };
        foreach (var ts in terrainSettings)
        {
            var prefabKeys = new List<string>();
            foreach (var enemy in ts.allowedEnemies) { prefabKeys.Add(enemy.prefabKey); }
            GetTerrainsData.allTerrainsData.Add
            (
                new TerrainData
                {
                    instanceKey = ts.terrainKey,
                    chunkPrefabKey = ts.chunkPrefab.prefabKey,
                    terrainType = ts.terrainType,
                    noiseMax = ts.noiseMax,
                    noiseMin = ts.noiseMin,
                    enemySpawnMult = ts.enemySpawnMult,
                    allowedEnemiesPrefabKeys = prefabKeys
                }
            );
        }
    }

    public static TerrainsData SetTerrainsData(TerrainsData terrainsData = null)
    {
        terrainsData ??= DS.GetSceneManager<SceneInitializeService>().MapInitializer.CurrentTerrainsData;
        GetTerrainsData = terrainsData;

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
            foreach (var terrainData in GetTerrainsData.allTerrainsData)
            {
                if (terrainSetting.terrainType != terrainData.terrainType) continue;
                terrainSetting.noiseMax = terrainData.noiseMax;
                terrainSetting.noiseMin = terrainData.noiseMin;
                terrainSetting.chunkPrefab.prefabKey = terrainData.chunkPrefabKey;
                terrainSetting.enemySpawnMult = terrainData.enemySpawnMult;
                terrainSetting.allowedEnemies.Clear();
                foreach (var key in terrainData.allowedEnemiesPrefabKeys)
                {
                    terrainSetting.allowedEnemies.Add(allEnemies[key]);
                }
            }
        }

        return GetTerrainsData;
    }
}
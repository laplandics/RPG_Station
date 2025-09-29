using System;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "GameSettings/GlobalSettings")]
public class GlobalSettings : ScriptableObject
{
    public MapSettingsSo mapSettings;
    public PlayerSettingsSo playerSettingsSo;
    public TerrainSettingsSo[] terrainSettings;

    public void OnEnable() => ResetSettings();

    [Button]
    private void ResetSettings()
    {
        TerrainDataHandler.SetTerrainSettings(terrainSettings);
        MapDataHandler.SetMapSettings(mapSettings);
        PlayerDataHandler.SetPlayerSettings(playerSettingsSo);
    }
}
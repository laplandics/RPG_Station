using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettings", menuName = "GameSettings/GlobalSettings")]
public class GlobalSettings : ScriptableObject
{
    public MapSettingsSo mapSettings;
    public PlayerSettings playerSettings;
    public TerrainSettingsSo[] terrainSettings;
}
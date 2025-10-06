using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "SomeTile", menuName = "GameSettings/Tiles/TileSettingsSo")]
public class TileSettingsSo : ScriptableObject
{
    public string tileKey;
    
    [Header("Tile Data")]
    public int tileAtlasIndex;
    public bool isUnreachable;
    [MinMaxSlider(0.0f, 1.0f)]public Vector2 noise;
    
    private void OnValidate()
    {
        var noiseX = Mathf.Round(noise.x * 100f) / 100f;
        var noiseY = Mathf.Round(noise.y * 100f) / 100f;
        noise = new Vector2(noiseX, noiseY);
    }

}
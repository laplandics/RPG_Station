using NaughtyAttributes;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[CreateAssetMenu(fileName = "SomeTileSet", menuName = "GameSettings/Tiles/New TileSet")]
public class TileSetSo : ScriptableObject
{
    public string tileKey;
    
    [Header("Tile Data")]
    public int[] tileAtlasIndexes;
    public bool isUnreachable;
    [MinMaxSlider(0.0f, 1.0f)]public Vector2 noise;
    
    private void OnValidate()
    {
        var noiseX = Mathf.Round(noise.x * 100f) / 100f;
        var noiseY = Mathf.Round(noise.y * 100f) / 100f;
        noise = new Vector2(noiseX, noiseY);
    }

    public int GetTileVariation(uint seed)
    {
        var rnd = new Random(seed);
        return tileAtlasIndexes[rnd.NextInt(0, tileAtlasIndexes.Length)];
    }
}
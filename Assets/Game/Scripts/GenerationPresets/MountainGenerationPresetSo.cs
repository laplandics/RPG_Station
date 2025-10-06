using UnityEngine;

[CreateAssetMenu(fileName = "MountainsGenerationPreset", menuName = "GameSettings/Biomes/MountainsGenerationPreset")]
public class MountainsGenerationPreset : ScriptableObject, IGenerationPreset
{
    public string presetName;

    [Header("Nosie Settings")]
    [SerializeField][Range(10000, 999999)] private int seed;
    [SerializeField][Range(0, 10)] private int octaves;
    [SerializeField][Range(0.00f, 5.0f)] private float persistence;
    [SerializeField][Range(0.00f, 5.0f)] private float lacunarity;
    [SerializeField][Range(0.00f, 1f)] private float scale;
    
    
    public float GetNoise(Vector2Int position)
    {
        var random = new System.Random(seed);
        var noiseX = random.Next(-10000, 10000);
        var noiseY = random.Next(-10000, 10000);
        return GetFractalNoise(position.x, position.y, noiseX, noiseY);
    }
    
    private float GetFractalNoise(int x, int y, float noiseX, float noiseY)
    {
        var total = 0f;
        var frequency = 1f;
        var amplitude = 1f;
        var maxValue = 0f;

        for (var i = 0; i < octaves; i++)
        {
            var fbmX = (x + noiseX) * scale/100 * frequency;
            var fbmY = (y + noiseY) * scale/100 * frequency;
            var perlinNoise = Mathf.PerlinNoise(fbmX, fbmY);
            var ridgedNoise = Ridged(perlinNoise);
            
            total += ridgedNoise * amplitude;
            maxValue += amplitude;
            
            amplitude *= persistence;
            frequency *= lacunarity;
        }
        
        var fbmNoise = total / maxValue;
        var stretchedFbmNosie = StretchContrast(fbmNoise, 0.05f, 1f);
        return stretchedFbmNosie;
    }
    
    private static float StretchContrast(float v, float low, float high)
    {
        var t = (v - low) / (high - low);
        return Mathf.Clamp01(t);
    }
    
    private static float Ridged(float noise01)
    {
        var n = noise01 * 2f - 1f;
        n = 1f - Mathf.Abs(n);
        return Mathf.Clamp01(n);
    }
}
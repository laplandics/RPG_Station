using UnityEngine;

public static class NoiseGenerator
{
    public static float GetPerlinNoise(float scale, float noiseX, float noiseY, int x, int y)
    {
        var noise = Mathf.PerlinNoise((x + noiseX) * scale/100, (y + noiseY) * scale/100);
        return noise;
    }

    public static float GetFractalNoise(int x, int y, int octaves, float persistence, float lacunarity, float scale, float noiseX, float noiseY)
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

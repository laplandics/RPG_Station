using UnityEngine;

public interface IGenerationPreset
{
    public float GetNoise(Vector2Int position);
}
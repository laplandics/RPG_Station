using UnityEngine;

[CreateAssetMenu(fileName = "TerrainTexturesHandler", menuName = "GameSettings/Terrain/TerrainTexturesHandler")]
public class TerrainRenderSettingsSo : ScriptableObject
{
    private static readonly int TerrainTextures = Shader.PropertyToID("_TerrainTextures");

    public Material testQuad;
    public Texture2D[] textures;
    [HideInInspector] public Texture2DArray textureArray;

    private void OnEnable()
    {
        EventManager.OnMapSpawned.RemoveListener(BuildArray);
        EventManager.OnMapSpawned.AddListener(BuildArray);
    }

    private void BuildArray(Map arg0, MapData arg1, AllTilesData arg2, AllTerrainData arg3)
    {
        if (textures == null || textures.Length == 0)
        {
            Debug.LogWarning("No source textures assigned!");
            return;
        }

        var width = textures[0].width;
        var height = textures[0].height;
        var format = textures[0].format;

        textureArray = new Texture2DArray(width, height, textures.Length, format, true, false);

        for (var i = 0; i < textures.Length; i++)
        {
            for (var mip = 0; mip < textures[i].mipmapCount; mip++) { Graphics.CopyTexture(textures[i], 0, mip, textureArray, i, mip); }
        }

        textureArray.Apply(false);
        Debug.Log($"Array size: {textureArray.depth}, resolution: {textureArray.width}x{textureArray.height}");
        testQuad.SetTexture(TerrainTextures, textureArray);
    }

    private void OnDisable() => EventManager.OnMapSpawned.RemoveListener(BuildArray);
}

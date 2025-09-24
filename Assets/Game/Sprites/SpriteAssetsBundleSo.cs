using System;
using UnityEngine;

public enum SpriteSheetType
{
    Head,
    Torso,
    Hands,
    Legs,
    Feet
}

[CreateAssetMenu(fileName = "SpriteAsset", menuName = "DataStorage/SpriteAsset")]
public class SpriteAssetsBundleSo : ScriptableObject
{
    [Serializable]
    public class Sprites
    {
        public SpriteSheetType type;
        public GameObject spriteObject;
        public string id;
    }
    public Sprites[] spritesStructs;
}

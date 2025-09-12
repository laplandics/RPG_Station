using System;
using UnityEngine;

public class PlayerPersistentData : MonoBehaviour, IPersistentData
{
    public void Save(ref SaveloadSystem.SaveData data)
    {
        var player = FindFirstObjectByType<Player>();
        if (!player) return;
        var playerTransform = player.transform;
        var playerSpriteRenderer = player.playerSpriteRenderer;
        
        data.playerData.position = playerTransform.position;
        data.playerData.rotationSprite = playerSpriteRenderer.sprite;
    }

    public void Load(SaveloadSystem.SaveData saveData)
    {
        var player = FindFirstObjectByType<Player>();
        if (!player) return;
        var playerTransform = player.transform;
        var playerSpriteRenderer = player.playerSpriteRenderer;
        
        playerTransform.position = saveData.playerData.position;
        playerSpriteRenderer.sprite = saveData.playerData.rotationSprite;
    }
}

[Serializable]
public struct PlayerSaveData
{
    public Vector3 position;
    public Sprite rotationSprite;
}
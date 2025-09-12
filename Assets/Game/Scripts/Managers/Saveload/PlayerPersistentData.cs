using UnityEngine;

public class PlayerPersistentData : MonoBehaviour, IPersistentData
{
    public void Save(SaveDataSO data)
    {
        var player = FindFirstObjectByType<Player>();
        if (!player) return;
        var playerTransform = player.transform;
        var playerSpriteRenderer = player.playerSpriteRenderer;
        
        data.saveData.playerSaveData.position = playerTransform.position;
        data.saveData.playerSaveData.rotationSprite = playerSpriteRenderer.sprite;
    }

    public void Load(SaveDataSO data)
    {
        var player = FindFirstObjectByType<Player>();
        if (!player) return;
        var playerTransform = player.transform;
        var playerSpriteRenderer = player.playerSpriteRenderer;
        
        playerTransform.position = data.saveData.playerSaveData.position;
        playerSpriteRenderer.sprite = data.saveData.playerSaveData.rotationSprite;
    }
}
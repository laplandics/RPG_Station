using UnityEngine;

public class PlayerSpriteSwapper : MonoBehaviour
{
    [SerializeField] private SpriteAssetsBundleSo spriteAssetsBundle;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Sprite baseLeft;
    [SerializeField] private Sprite baseRight;

    private PlayerManagerSo _playerManager;

    public void Initialize()
    {
        _playerManager = DS.GetSoManager<PlayerManagerSo>();
    }

    public void SetPlayerSprite()
    {
        var input = _playerManager.MoveInput;
        if (input == Vector2.zero) return;
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) { spriteRenderer.sprite = input.x > 0 ? baseRight : baseLeft; }
        else if (Mathf.Abs(input.x) == Mathf.Abs(input.y)) {spriteRenderer.sprite = input.x > 0 ? baseRight : baseLeft;}
    }
}

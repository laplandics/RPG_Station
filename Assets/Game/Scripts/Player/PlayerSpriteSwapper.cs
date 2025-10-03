using UnityEngine;

public class PlayerSpriteSwapper : MonoBehaviour
{
    [SerializeField] private Sprite baseLeft;
    [SerializeField] private Sprite baseRight;
    private SpriteRenderer _spriteRenderer;

    private PlayerInitializer _playerManager;

    public void Initialize(PlayerInitializer initializer, Player player)
    {
        _playerManager  = initializer;
        _spriteRenderer = player.playerSpriteRenderer;
        DS.GetSceneManager<RoutineService>().GetUpdateAction(SetPlayerSprite);
    }

    private void SetPlayerSprite()
    {
        var input = _playerManager.MoveInput;
        if (input == Vector2.zero) return;
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y) || Mathf.Approximately(Mathf.Abs(input.x), Mathf.Abs(input.y)))
        {
            _spriteRenderer.sprite = input.x > 0 ? baseRight : baseLeft;
        }
    }

    private void OnDestroy() => DS.GetSceneManager<RoutineService>().ReturnUpdateAction(SetPlayerSprite);
}

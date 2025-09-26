using UnityEngine;

public class PlayerSpriteSwapper : MonoBehaviour
{
    [SerializeField] private Sprite baseLeft;
    [SerializeField] private Sprite baseRight;
    private SpriteRenderer _spriteRenderer;

    private PlayerManagerSo _playerManager;

    public void Initialize()
    {
        _playerManager = DS.GetObjectManager<PlayerManagerSo>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        DS.GetSceneManager<RoutineManager>().GetUpdateAction(SetPlayerSprite);
    }

    public void SetPlayerSprite()
    {
        var input = _playerManager.MoveInput;
        if (input == Vector2.zero) return;
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) { _spriteRenderer.sprite = input.x > 0 ? baseRight : baseLeft; }
        else if (Mathf.Abs(input.x) == Mathf.Abs(input.y)) { _spriteRenderer.sprite = input.x > 0 ? baseRight : baseLeft; }
    }

    private void OnDestroy() => DS.GetSceneManager<RoutineManager>().ReturnUpdateAction(SetPlayerSprite);
}

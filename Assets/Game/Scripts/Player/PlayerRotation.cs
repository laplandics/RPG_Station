using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSr;
    [SerializeField] private Sprite playerBack;
    [SerializeField] private Sprite playerFront;
    [SerializeField] private Sprite playerLeft;
    [SerializeField] private Sprite playerRight;
    private PlayerController _playerController;
    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!_playerController) return;
        var rawInput = _playerController.GetMoveInput();
        if (rawInput == Vector2.zero) return;
        if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
        {
            if (rawInput.x > 0) playerSr.sprite = playerRight;
            else playerSr.sprite =  playerLeft;
        }
        else if (Mathf.Abs(rawInput.x) < Mathf.Abs(rawInput.y))
        {
            if (rawInput.y > 0) playerSr.sprite = playerBack;
            else playerSr.sprite =  playerFront;
        }
    }
}

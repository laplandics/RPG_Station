using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private GameInputs _input;
    private Vector2 _moveInput;
    private bool _isBlocked;
    
    public void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _input = DS.GetSoManager<GlobalInputsManagerSo>().GetInputs();
        
        _input.Player.Move.Enable();
        _input.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
    }

    public void SetLockState(bool isLocked) => _isBlocked = isLocked;

    private void OnDestroy()
    {
        _input?.Player.Move.Disable();
    }

    public void MovePlayer()
    {
        if (_isBlocked) return;
        var move = new Vector2(_moveInput.x, _moveInput.y) * (speed * Time.deltaTime);
        var movePos = new Vector2(transform.position.x + move.x, transform.position.y + move.y);
        _rb.MovePosition(movePos);
    }
}

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float cellSize;
    [SerializeField] private float baseStepsDelay;
    [SerializeField] private float minStepsDelay;
    [SerializeField] private float decreaseRate;
    private bool _isBlocked;
    private Rigidbody2D _rb;
    private GameInputs _input;
    private Vector2 _moveInput;
    private Vector2 _targetPosition;
    private bool _isMoving;
    private Vector2 _currentInput;
    private float _holdTime;
    
    public void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _input = DS.GetSoManager<GlobalInputsManagerSo>().GetInputs();
        
        _input.Player.Move.Enable();
        _input.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Move.performed += _ => DS.GetSceneManager<RoutineManager>().StartRoutine(MovePlayer());
        _input.Player.Move.canceled += _ => _moveInput = Vector2.zero;
    }

    private Vector2 SnapToGrid(Vector2 pos)
    {
        var x = Mathf.Round(pos.x / cellSize) * cellSize;
        var y = Mathf.Round(pos.y / cellSize) * cellSize;
        return new Vector2(x, y);
    }

    private Vector2 SnapToCellCenter(Vector2 pos)
    {
        var x = Mathf.Round(pos.x / cellSize) * cellSize + 0.5f;
        var y = Mathf.Round(pos.y / cellSize) * cellSize + 0.5f;
        return new Vector2(x, y);
    }

    private IEnumerator MovePlayer()
    {
        if (_isMoving) yield break;
        _isMoving = true;
        while (true)
        {
            yield return new WaitUntil(() => !_isBlocked);
            _currentInput = _moveInput;
            if (_currentInput == Vector2.zero)
            {
                _holdTime = 0f;
                yield return null;
                continue;
            }

            if(!TryCalculateNewPosition()) yield break;
            yield return DS.GetSceneManager<RoutineManager>().StartRoutine(ChangePlayersPosition());
            DS.GetSoManager<EventManagerSo>().onPlayersPositionChanged?.Invoke();
            yield return null;
        }
    }

    private bool TryCalculateNewPosition()
    {
        var currentPosition = SnapToGrid(transform.position);
        var pos = currentPosition + _moveInput * cellSize;
        _targetPosition = SnapToGrid(pos);
        if (!Physics2D.OverlapCircle(SnapToCellCenter(_targetPosition), cellSize * 0.1f, LayerMask.NameToLayer("Obstacle"))) return true;
        print("The place is taken");
        _isMoving = false;
        return false;
    }
    
    private IEnumerator ChangePlayersPosition()
    {
        while ((Vector2)transform.position != _targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _targetPosition) < 0.01f)
            {
                transform.position = _targetPosition;
                yield return new WaitForSeconds(CalculateDelay());
                break;
            }
            yield return null;
        }
    }

    private float CalculateDelay()
    {
        _holdTime += Time.deltaTime;
        var delay = Mathf.Clamp(baseStepsDelay - _holdTime * decreaseRate, minStepsDelay, baseStepsDelay);
        return delay;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(SnapToCellCenter(_targetPosition), cellSize*0.1f);
    }

    public void SetLockState(bool isBlocked) => _isBlocked = isBlocked;

    private void OnDestroy() => _input?.Player.Move.Disable();

    public Vector2 GetMoveInput() => _currentInput;
}

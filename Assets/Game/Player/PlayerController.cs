using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float animationSpeed;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float baseStepsDelay;
    [SerializeField] private float minStepsDelay;
    [SerializeField] private float decreaseRate;
    private GameInputs _input;
    private Vector2 _moveInput;
    private Vector2 _targetPosition;
    private Vector2 _currentInput;
    private bool _isCancelled;
    private bool _isBlocked;
    private bool _isMoving;
    private float _holdTime;
    
    public void Initialize()
    {
        _input = DS.GetSoManager<GlobalInputsManagerSo>().GetInputs();
        
        _input.Player.Move.Enable();
        _input.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Move.performed += _ => DS.GetSceneManager<RoutineManager>().StartRoutine(MovePlayer());
        _input.Player.Move.canceled += _ => _moveInput = Vector2.zero;
    }

    private IEnumerator MovePlayer()
    {
        if (_isMoving) yield break;
        _isMoving = true;
        _isCancelled = false;
        while (!_isCancelled)
        {
            yield return new WaitUntil(() => !_isBlocked);
            _currentInput = _moveInput;
            if (_currentInput == Vector2.zero)
            {
                _holdTime = 0f;
                yield return null;
                continue;
            }

            if (!TryCalculateNewPosition()) yield break;
            IEnumerator action = null;
            yield return new MoveActionTc().PerformAction(0.6f, 1, ChangePlayersPosition(), CancelMovement()).ToCoroutine(result => action = result);
            yield return DS.GetSceneManager<RoutineManager>().StartRoutine(action);
            yield return null;
        }
    }

    private bool TryCalculateNewPosition()
    {
        var currentPosition = GridMover.SnapToGrid(transform.position);
        var pos = currentPosition + _moveInput * GridMover.CELL_SIZE;
        _targetPosition = GridMover.SnapToGrid(pos);
        if (!Physics2D.OverlapCircle(GridMover.SnapToCellCenter(_targetPosition), GridMover.CELL_SIZE * 0.1f, obstacleMask)) return true;
        _isMoving = false;
        return false;
    }

    private IEnumerator CancelMovement()
    {
        _isCancelled = true;
        _isMoving = false;
        yield break;
    }

    private IEnumerator ChangePlayersPosition()
    {
        while ((Vector2)transform.position != _targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, animationSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _targetPosition) < 0.01f)
            {
                transform.position = _targetPosition;
                yield return new WaitForSeconds(CalculateDelay());
                break;
            }
            yield return null;
        }
        DS.GetSoManager<EventManagerSo>().onPlayersPositionChanged?.Invoke(transform);
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
        Gizmos.DrawWireSphere(GridMover.SnapToCellCenter(_targetPosition), GridMover.CELL_SIZE * 0.1f);
    }

    public void SetLockState(bool isBlocked) => _isBlocked = isBlocked;

    private void OnDestroy() => _input?.Player.Move.Disable();

    public Vector2 GetMoveInput() => _currentInput;
}

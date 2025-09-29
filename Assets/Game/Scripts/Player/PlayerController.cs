using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static EventManager;
using static UnityEngine.InputSystem.InputAction;
using static PlayerDataHandler;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    private GameInputs _input;
    private Transform _playerTransform;
    private Vector2 _moveInput;
    private Vector2Int _targetPosition;
    private Vector2 _currentInput;
    private bool _isCancelled;
    private bool _isBlocked;
    private bool _isMoving;
    private float _holdTime;
    private bool _isInitialized;

    public void Initialize()
    {
        _input = DS.GetGlobalManager<GlobalInputsManagerSo>().GetInputs();
        _playerTransform = transform;

        _input.Player.Move.Enable();
        _input.Player.Move.performed += ReadInput;
        _input.Player.Move.performed += StartMoving;
        _input.Player.Move.canceled += ResetInput;

        _isInitialized = true;
    }
    private void ReadInput(CallbackContext ctx) => _moveInput = ctx.ReadValue<Vector2>();
    private void StartMoving(CallbackContext _) => DS.GetSceneManager<RoutineService>().StartRoutine(MovePlayer());
    private void ResetInput(CallbackContext _) => _moveInput = Vector2.zero;

    private IEnumerator MovePlayer()
    {
        if (!_isInitialized) yield break;
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
            yield return DS.GetSceneManager<RoutineService>().StartRoutine(action);
            yield return null;
        }
    }

    private bool TryCalculateNewPosition()
    {
        var currentPosition = GridMover.SnapToGrid(_playerTransform.position);
        var pos = currentPosition + _moveInput * GridMover.CellSize;
        _targetPosition = GridMover.SnapToGrid(pos);
        if (!Physics2D.OverlapCircle(GridMover.SnapToCellCenter(_targetPosition), GridMover.CellSize * 0.1f, obstacleMask)) return true;
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
        while ((Vector2)_playerTransform.position != _targetPosition)
        {
            _playerTransform.position = Vector2.MoveTowards(_playerTransform.position, _targetPosition, GetPlayerData.animationSpeed * Time.deltaTime);
            if (Vector2.Distance(_playerTransform.position, _targetPosition) < 0.01f)
            {
                _playerTransform.position = new Vector2(_targetPosition.x, _targetPosition.y);
                yield return new WaitForSeconds(CalculateDelay());
                break;
            }
            yield return null;
        }
        OnPlayersPositionChanged?.Invoke(_playerTransform);
    }

    private float CalculateDelay()
    {
        _holdTime += Time.deltaTime;
        var delay = Mathf.Clamp(GetPlayerData.baseStepsDelay - _holdTime * GetPlayerData.decreaseRate, GetPlayerData.minStepsDelay, GetPlayerData.baseStepsDelay);
        return delay;
    }

    private void OnDestroy()
    {
        if (!_isInitialized) return;
        _input.Player.Move.performed -= ReadInput;
        _input.Player.Move.performed -= StartMoving;
        _input.Player.Move.canceled -= ResetInput;
        _input?.Player.Move.Disable();
        _isInitialized = false;
    }

    public Vector2 GetMoveInput() => _currentInput;
}

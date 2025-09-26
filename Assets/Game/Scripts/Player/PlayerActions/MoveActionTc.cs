using Cysharp.Threading.Tasks;
using UnityEngine;
using static EventManager;

public class MoveActionTc : ITimeConsumableAction
{
    public async UniTask<T> PerformAction<T>(float speed, int unit, T requestedAction, T cancelledAction)
    {
        if (await CancelOperationRequest()) return cancelledAction;
        CalculateConsumedTime(speed, unit);
        return requestedAction;
    }
    public void CalculateConsumedTime(float speed, int unit)
    {
        onTimePassed?.Invoke(unit / speed);
    }

    public async UniTask<bool> CancelOperationRequest()
    {
        Debug.LogWarning("TODO: ask player to cancel action if enemy near");
        await UniTask.Yield();
        return false;
    }
}

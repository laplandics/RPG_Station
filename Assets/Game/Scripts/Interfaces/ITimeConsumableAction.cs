using Cysharp.Threading.Tasks;

public interface ITimeConsumableAction
{
    public UniTask<bool> CancelOperationRequest();
    public void CalculateConsumedTime(float speed, int unit);
    public UniTask<T> PerformAction<T>(float speed, int unit, T requestedAction, T cancelledAction);
}
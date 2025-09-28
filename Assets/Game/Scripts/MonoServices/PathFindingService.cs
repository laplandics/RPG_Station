using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PathFindingService : MonoBehaviour, IInSceneService
{
    public void Initialize() { }
    public async UniTask<List<Vector3>> FindPath(Vector3 currentPosition, Vector3 targetPosition)
    {
        var nearestPoint = Vector3.zero;
        var lastDistance = float.MaxValue;
        var routePoints = new List<Vector3>();
        while (nearestPoint != targetPosition)
        {
            for (var y = -GridMover.CellSize; y <= GridMover.CellSize; y++)
            {
                for (var x = -GridMover.CellSize; x <= GridMover.CellSize; x++)
                {
                    var point = new Vector3(currentPosition.x + x, currentPosition.y + y, 0f);
                    var distance = Vector3.Distance(point, targetPosition);
                    if (x != 0 && y != 0) distance *= 1.5f; 
                    if (distance < lastDistance)
                    {
                        lastDistance = distance;
                        nearestPoint = point;
                    }
                }
            }
            routePoints.Add(nearestPoint);
            currentPosition = nearestPoint;

            await UniTask.Yield();
        }

        return routePoints;
    }
}

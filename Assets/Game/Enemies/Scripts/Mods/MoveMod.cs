using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class MoveMod : IEnemyMod, ISaveAble
{
    public string key;
    public float moveSpeed;
    public Vector2 routFindArea;
    private Vector3 targetPosition;
    private List<Vector3> _routePoints = new();
    private Coroutine _moveRoutine;
    private int _chunkSize;
    private bool _isMoving;
    private bool _isRoutCalculated;

    public Enemy Owner { get; set; }
    public string InstanceKey { get => key; set => key = value; }
    public MoveModData ModData { get; set; }

    public void LoadMod()
    {
        DS.GetSoManager<EventManagerSo>().onTimePassed.AddListener(StartMovingOnTimePassed);
        _chunkSize = DS.GetSoManager<ChunksManagerSo>().ChunkSize;
    }
    public void UnloadMod()
    {
        DS.GetSoManager<EventManagerSo>().onTimePassed.RemoveListener(StartMovingOnTimePassed);
        if (_moveRoutine != null && DS.GetSceneManager<RoutineManager>() != null) DS.GetSceneManager<RoutineManager>().EndRoutine(_moveRoutine);
    }
    public ModData GetModData() => ModData;
    public void SetModData(ModData data) => ModData = (MoveModData)data;
    public void Save() { var data = new MoveModData { key = key, moveSpeedData = moveSpeed, routFindAreaData = routFindArea }; ModData = data; }
    public void Load() { key = ModData.key; moveSpeed = ModData.moveSpeedData; routFindArea = ModData.routFindAreaData; }

    private void StartMovingOnTimePassed(float time) { _moveRoutine = DS.GetSceneManager<RoutineManager>().StartRoutine(Move(time)); }
    private IEnumerator Move(float time)
    {
        var ownerTransform = Owner.transform;
        if (!_isMoving)
        {
            _isRoutCalculated = false;
            _isMoving = true;
            targetPosition = GetRandomPosition(GetCurrentChunk(ownerTransform));
            _routePoints = new List<Vector3>();
            yield return DS.GetSceneManager<PathFindingManager>().FindPath(ownerTransform.position, targetPosition).ToCoroutine(result => _routePoints = result);
            _isRoutCalculated = true;
        }
        if (!_isRoutCalculated) yield break;
        var difference = 0f;
        for (var i = 0; i < GridMover.GetCellCount(time, moveSpeed, Owner.BuferTime, out difference); i++)
        {
            if (_routePoints.Count == 0)
            {
                _isRoutCalculated = false;
                _isMoving = false;
                break;
            }
            if (ownerTransform == null) yield break;
            ownerTransform.position = _routePoints[0];
            _routePoints.RemoveAt(0);
        }
        Owner.BuferTime += difference;
        if (ownerTransform.position == targetPosition) _isMoving = false;
    }

    private Vector3 GetCurrentChunk(Transform ownerTransform)
    {
        var position = ownerTransform.position;
        var chunkX = (int)Math.Round(position.x / _chunkSize);
        var chunkY = (int)Math.Round(position.y / _chunkSize);

        return new Vector3(chunkX, chunkY);
    }

    private Vector3 GetRandomPosition(Vector3 currentChunk)
    {
        var maxY = (currentChunk.y + routFindArea.y) * _chunkSize;
        var minY = (currentChunk.y - routFindArea.y) * _chunkSize;
        var maxX = (currentChunk.x + routFindArea.x) * _chunkSize;
        var minX = (currentChunk.x - routFindArea.x) * _chunkSize;

        var posY = (int)Random.Range(minY, maxY);
        var posX = (int)Random.Range(minX, maxX);

        return new Vector3(posX, posY, 0);
    }

}

[Serializable]
public class MoveModData : ModData
{
    public float moveSpeedData;
    public Vector2 routFindAreaData;
}
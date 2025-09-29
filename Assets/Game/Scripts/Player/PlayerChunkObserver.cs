using System;
using UnityEngine;
using static EventManager;
using static GlobalMapMethods;

public class PlayerChunkObserver : IDisposable
{
    private Vector2Int _currentChunk;
    public PlayerChunkObserver(Transform playerTransform)
    {
        _currentChunk = GetPlayerChunk(playerTransform);
        OnPlayerChunkChanged?.Invoke(_currentChunk);
        OnPlayersPositionChanged.AddListener(InvokeOnPlayerChunkChanged);
    }
    
    private void InvokeOnPlayerChunkChanged(Transform playerTransform)
    {
        var newChunk = GetPlayerChunk(playerTransform);
        if (_currentChunk == newChunk) return;
        _currentChunk = newChunk;
        OnPlayerChunkChanged?.Invoke(_currentChunk);
    }

    public void Dispose()
    {
        OnPlayersPositionChanged.RemoveListener(InvokeOnPlayerChunkChanged);
    }
}
using System;
using UnityEngine;
using static EventManager;
using static GlobalMapMethods;

public class PlayerChunkObserver : IDisposable
{
    private Vector2Int _currentChunk;
    private readonly PlayerController _playerController;
    
    public PlayerChunkObserver(Transform playerTransform, PlayerController playerController)
    {
        _currentChunk = GetEntityChunk(playerTransform);
        _playerController = playerController;
        _playerController.UnreachableTiles.Clear();
        
        OnSmbEnteredChunk?.Invoke(_currentChunk, _playerController);
        OnPlayersPositionChanged.AddListener(InvokeOnPlayerChunkChanged);
    }

    private void InvokeOnPlayerChunkChanged(Transform playerTransform)
    {
        var newChunk = GetEntityChunk(playerTransform);
        if (_currentChunk == newChunk) return;
        _playerController.UnreachableTiles.Clear();
        _currentChunk = newChunk;
        OnSmbEnteredChunk?.Invoke(_currentChunk, _playerController);
    }
    
    public void Dispose()
    {
        OnPlayersPositionChanged.RemoveListener(InvokeOnPlayerChunkChanged);
    }
}
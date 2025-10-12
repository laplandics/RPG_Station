using System;
using UnityEngine;
using static EventManager;
using static MapHelper;

public class PlayerChunkObserver : IDisposable
{
    private Vector2Int _currentChunk;
    private readonly Player _player;
    
    public PlayerChunkObserver(Transform playerTransform, Player player)
    {
        _currentChunk = GetEntityChunk(playerTransform);
        _player = player;
        _player.UnreachableTiles.Clear();
        
        OnSmbEnteredChunk?.Invoke(_currentChunk, _player);
        OnPlayersPositionChanged.AddListener(InvokeOnPlayerChunkChanged);
    }

    private void InvokeOnPlayerChunkChanged(Transform playerTransform)
    {
        var newChunk = GetEntityChunk(playerTransform);
        if (_currentChunk == newChunk) return;
        _player.UnreachableTiles.Clear();
        _currentChunk = newChunk;
        OnSmbEnteredChunk?.Invoke(_currentChunk, _player);
    }
    
    public void Dispose()
    {
        OnPlayersPositionChanged.RemoveListener(InvokeOnPlayerChunkChanged);
    }
}
using UnityEngine;
using static EventManager;
using static GlobalMapMethods;

public class PlayerChunkObserver
{
    private Vector2Int _currentChunk;
    public PlayerChunkObserver(Transform playerTransform)
    {
        _currentChunk = GetPlayerChunk(playerTransform);
        OnPlayerChunkChanged?.Invoke(_currentChunk);
        OnPlayersPositionChanged.AddUniqueListener(InvokeOnPlayerChunkChanged);
    }
    
    private void InvokeOnPlayerChunkChanged(Transform playerTransform)
    {
        var newChunk = GetPlayerChunk(playerTransform);
        if (_currentChunk == newChunk) return;
        _currentChunk = newChunk;
        OnPlayerChunkChanged?.Invoke(_currentChunk);
    }
}
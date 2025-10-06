using System;
using System.Collections.Generic;
using UnityEngine;

public static class MemorizedAreaHandler
{
    private static int _memorizedArea;
    
    public static void RestoreMemorizedArea(Queue<Vector2Int> memorizedChunks, Action<Vector2Int> generationAction, int area)
    {
        _memorizedArea = area;
        if (memorizedChunks.Count <= 0) return;
        foreach (var position in memorizedChunks)
        {
            var vectorPos = new Vector2Int(position.x, position.y);
            generationAction(vectorPos);
        }
    }
    
    public static void UpdateMemorizedArea(Queue<Vector2Int> memorizedChunks, Vector2Int index)
    {
        if (memorizedChunks.Contains(index)) return;
        memorizedChunks.Enqueue(index);
        if (memorizedChunks.Count > _memorizedArea) memorizedChunks.Dequeue();
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static GridMover;
using static MapDataHandler;

public static class GlobalMapMethods
{
    public static Vector2Int GetPlayerChunk(Transform playerTransform)
    {
        var player = playerTransform.position;
        var playerChunkX = (int)Math.Round(player.x / GetMapData.chunkSize);
        var playerChunkY = (int)Math.Round(player.y / GetMapData.chunkSize);
    
        return new Vector2Int(playerChunkX, playerChunkY);
    }
    
    public static Vector2Int GetChunkCenter(Vector2 pos)
    {
        var chunkX = (int)Math.Round(pos.x / GetMapData.chunkSize) * GetMapData.chunkSize;
        var chunkY = (int)Math.Round(pos.y / GetMapData.chunkSize) * GetMapData.chunkSize;
    
        return new Vector2Int(chunkX, chunkY);
    }

    public static Vector2Int[] GetChunksAroundPlayer(int area, int distance, Vector2Int playerChunk)
    {
        var allChunksInArea = GetBoundaries(area, playerChunk);
        var farChunks = new List<Vector2Int>();
        foreach (var pos in allChunksInArea)
        {
            if(Mathf.Abs(pos.x - playerChunk.x) <= distance * GetMapData.chunkSize && Mathf.Abs(pos.y - playerChunk.y) <= distance * GetMapData.chunkSize) continue;
            farChunks.Add(pos);
        }
        return farChunks.ToArray();
    }

    public static List<Vector2Int> GetBoundaries(int area, Vector2Int center)
    {
        var boundaries = new List<Vector2Int>();
        for (var y = -area; y <= area; y++)
        {
            for (var x = -area; x <= area; x++)
            {
                var chunkX = center.x + x;
                var chunkY = center.y + y;
                boundaries.Add(new Vector2Int(chunkX * GetMapData.chunkSize, chunkY * GetMapData.chunkSize));
            }
        }
        return boundaries;
    }
    
    public static Vector2 GetRandomSpawnPoint(Vector2Int chunkPosition)
    {
        var halfWidth = GetMapData.chunkSize / 2f;
        var halfHeight = GetMapData.chunkSize / 2f;

        var x = Random.Range(chunkPosition.x - halfWidth, chunkPosition.x + halfWidth);
        var y = Random.Range(chunkPosition.y - halfHeight, chunkPosition.y + halfHeight);
        
        return SnapToGrid(new Vector2(x, y));
    }
}
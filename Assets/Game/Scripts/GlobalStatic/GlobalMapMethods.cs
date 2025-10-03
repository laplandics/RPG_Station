using System.Collections.Generic;
using UnityEngine;
using static MapDataHandler;

public static class GlobalMapMethods
{
    public static Vector2Int GetEntityChunk(Transform entityTransform)
    {
        var entityTransformPosition = entityTransform.position;
        var entityChunkX = Mathf.FloorToInt(entityTransformPosition.x/ GetMapData.chunkSize);
        var entityChunkY = Mathf.FloorToInt(entityTransformPosition.y / GetMapData.chunkSize);
    
        return new Vector2Int(entityChunkX, entityChunkY);
    }

    public static Vector2Int GetChunkCenterWorldPosition(Vector2Int chunk)
    {
        var chunkXPos = chunk.x * GetMapData.chunkSize;
        var chunkYPos = chunk.y * GetMapData.chunkSize;
        return new Vector2Int(chunkXPos, chunkYPos);
    }

    public static List<Vector2Int> GetChunksIndexesInArea(int chunksCount, Vector2Int index)
    {
        var positions = new List<Vector2Int>();
        var halfArea = chunksCount / 2;
        
        var startX = index.x - halfArea;
        var startY = index.y - halfArea;
        
        for (var y = 0; y < chunksCount; y ++)
        {
            for (var x = 0; x < chunksCount; x ++)
            {
                var xPos = startX + x;
                var yPos = startY + y;
                positions.Add(new Vector2Int(xPos, yPos));
            }
        }
        return positions;
    }

    public static List<Vector2Int> GetTilesPositionsInChunk(Vector2Int chunkIndex)
    {
        var positions = new List<Vector2Int>();

        int startX = chunkIndex.x * GetMapData.chunkSize;
        int startY = chunkIndex.y * GetMapData.chunkSize;

        for (int y = 0; y < GetMapData.chunkSize; y++)
        {
            for (int x = 0; x < GetMapData.chunkSize; x++)
            {
                positions.Add(new Vector2Int(startX + x, startY + y));
            }
        }
        return positions;
    }
}
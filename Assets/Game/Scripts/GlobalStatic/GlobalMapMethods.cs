using System.Collections.Generic;
using UnityEngine;
using static GameDataInjector;

public static class GlobalMapMethods
{
    public static Vector2Int GetEntityChunk(Transform entityTransform)
    {
        var entityTransformPosition = entityTransform.position;
        var entityChunkX = Mathf.FloorToInt(entityTransformPosition.x/ InjectMapData.chunkSize);
        var entityChunkY = Mathf.FloorToInt(entityTransformPosition.y / InjectMapData.chunkSize);
    
        return new Vector2Int(entityChunkX, entityChunkY);
    }

    public static Vector2Int GetChunkCenterWorldPosition(Vector2Int chunk)
    {
        var chunkXPos = chunk.x * InjectMapData.chunkSize;
        var chunkYPos = chunk.y * InjectMapData.chunkSize;
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

        var startX = chunkIndex.x * InjectMapData.chunkSize;
        var startY = chunkIndex.y * InjectMapData.chunkSize;

        for (var y = 0; y < InjectMapData.chunkSize; y++)
        {
            for (var x = 0; x < InjectMapData.chunkSize; x++)
            {
                positions.Add(new Vector2Int(startX + x, startY + y));
            }
        }
        return positions;
    }
}
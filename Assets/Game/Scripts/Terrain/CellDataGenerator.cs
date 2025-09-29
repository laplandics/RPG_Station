using UnityEngine;
using static MapDataHandler;

public class CellDataGenerator
{ 
    // private void GenerateCellsData()
    // {
    //     var tiles = new int[GetMapData.mapSize, GetMapData.mapSize];
    //     var random = new System.Random(GetMapData.seed);
    //     var noiseX = random.Next(-10000, 10000);
    //     var noiseY = random.Next(-10000, 10000);
    //     var maxTileIndex = atlasColumns * atlasRows;
    //     for (var y = 0; y < mapSizeY; y++)
    //     {
    //         for (var x = 0; x < mapSizeX; x++)
    //         {
    //             var noise = Mathf.PerlinNoise((x + noiseX) * noiseScaleDividedBy100/100, (y + noiseY) * noiseScaleDividedBy100/100);
    //             var tileIndex = Mathf.Clamp(Mathf.FloorToInt(noise * maxTileIndex), 0, maxTileIndex - 1);
    //             _tiles[x, y] = tileIndex;
    //         }
    //     }
    // }
}
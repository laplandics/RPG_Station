using UnityEngine;

public static class Grid
{
    public const int TileSize = 1;

    public static Vector2Int SnapToGrid(Vector2 pos)
    {
        var x = Mathf.RoundToInt(pos.x / TileSize) * TileSize;
        var y = Mathf.RoundToInt(pos.y / TileSize) * TileSize;
        return new Vector2Int(x, y);
    }

    public static Vector2 SnapToCellCenter(Vector2 pos)
    {
        var x = Mathf.RoundToInt(pos.x / TileSize) * TileSize + 0.5f;
        var y = Mathf.RoundToInt(pos.y / TileSize) * TileSize + 0.5f;
        return new Vector2(x, y);
    }

    public static int GetCellCount(float passedTime, float speed, float currentAccumulatedTime, out float difference)
    {
        var accumulatedTime = currentAccumulatedTime + passedTime;
        var cells = (int)(accumulatedTime / speed);
        difference = passedTime - cells;
        return cells;
    }
}

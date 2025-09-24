using UnityEngine;

public static class GridMover
{
    public const int CELL_SIZE = 1;

    public static Vector2Int SnapToGrid(Vector2 pos)
    {
        var x = Mathf.RoundToInt(pos.x / CELL_SIZE) * CELL_SIZE;
        var y = Mathf.RoundToInt(pos.y / CELL_SIZE) * CELL_SIZE;
        return new Vector2Int(x, y);
    }

    public static Vector2 SnapToCellCenter(Vector2 pos)
    {
        var x = Mathf.RoundToInt(pos.x / CELL_SIZE) * CELL_SIZE + 0.5f;
        var y = Mathf.RoundToInt(pos.y / CELL_SIZE) * CELL_SIZE + 0.5f;
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

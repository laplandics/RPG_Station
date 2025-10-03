using System.Collections.Generic;
using UnityEngine;

public interface IWalkable
{
    public HashSet<Vector2Int> UnreachableTiles { get; set; }
    public bool CheckTilePosition();
}
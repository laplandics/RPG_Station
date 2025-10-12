using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IWalkable
{
    public PlayerController playerController;
    public PlayerSpriteSwapper playerSpriteSwapper;
    public SpriteRenderer playerSpriteRenderer;
    
    public HashSet<Vector2Int> UnreachableTiles { get; set; } = new();
    public bool CheckTilePosition()
    {
        var currentPosition = Grid.SnapToGrid(transform.position);
        var pos = currentPosition + playerController.GetMoveInput() * Grid.TileSize;
        playerController.TargetPosition = Grid.SnapToGrid(pos);
        if (!UnreachableTiles.Contains(playerController.TargetPosition)) return true;
        playerController.IsMoving = false;
        return false;
    }
}
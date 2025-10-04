using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileSet", menuName = "GameSettings/TileSet")]
public class TileSetSo : ScriptableObject
{
    public TileBase[] tiles;
}
using UnityEngine;

public interface ISortingOnLayerObject
{
    public void AddSelfIntoSortingOrderManager();
    
    public void RemoveSelfFromSortingOrderManager();
    public SpriteRenderer SpriteRenderer { get; }
    public float YCoordinate { get; }
}

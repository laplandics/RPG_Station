using UnityEngine;

public class Player : MonoBehaviour, ISortingOnLayerObject
{
    public PlayerController controller;
    public SpriteRenderer playerSpriteRenderer;

    private void Start()
    {
        AddSelfIntoSortingOrderManager();
    }
    
    public void AddSelfIntoSortingOrderManager()
    {
        SortingOrderManager.Instance.AddSortingOnLayerObject(this);
    }

    public void RemoveSelfFromSortingOrderManager()
    {
        SortingOrderManager.Instance.RemoveSortingOnLayerObject(this);
    }

    private void OnDestroy()
    {
        RemoveSelfFromSortingOrderManager();
    }
    
    public SpriteRenderer SpriteRenderer => playerSpriteRenderer;
    public float YCoordinate => transform.position.y;
}

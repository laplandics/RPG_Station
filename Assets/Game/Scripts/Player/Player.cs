using UnityEngine;

public class Player : MonoBehaviour, ISortingOnLayerObject
{
    public PlayerController controller;
    public SpriteRenderer playerSpriteRenderer;

    private void Start()
    {
        AddSelfIntoSortingOrderManager();
        DS.GetManager<GameManagerSO>().OnSave.AddListener(ShareData);
    }
    
    public void AddSelfIntoSortingOrderManager()
    {
        DS.GetManager<SortingOrderManagerSO>().AddSortingOnLayerObject(this);
    }

    public void RemoveSelfFromSortingOrderManager()
    {
        DS.GetManager<SortingOrderManagerSO>().RemoveSortingOnLayerObject(this);
    }

    private void OnDisable()
    {
        DS.GetManager<GameManagerSO>().OnSave.RemoveListener(ShareData);
    }
    
    private void OnDestroy()
    {
        RemoveSelfFromSortingOrderManager();
    }
    
    public void ShareData()
    {
        
    }
    public SpriteRenderer SpriteRenderer => playerSpriteRenderer;
    public float YCoordinate => transform.position.y;
}

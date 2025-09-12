using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ISortingOnLayerObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private string id;
    [SerializeField] bool isFriendly;
    private bool _isDead;

    private void OnEnable()
    {
        AddSelfIntoSortingOrderManager();
    }
    
    private void OnCollisionEnter2D(Collision2D player)
    {
        Debug.Log("OnCollisionEnter2D");
        if (_isDead || isFriendly) return;
        var playerData = player.gameObject.GetComponent<Player>();
        if (!playerData) return;
        StartCoroutine(WaitForNewEnemies(playerData));
    }

    private IEnumerator WaitForNewEnemies(Player player)
    {
        player.controller.SetLockState(true);
        
        yield return new WaitForSeconds(0.3f);
        
        StartFighting();
    }
    
    private void StartFighting()
    {
        
    }
    
    public void SetState(bool isDead)
    {
        _isDead = isDead;
        gameObject.SetActive(!_isDead);
    }

    public void AddSelfIntoSortingOrderManager()
    {
        SortingOrderManager.Instance.AddSortingOnLayerObject(this);
    }

    public void RemoveSelfFromSortingOrderManager()
    {
        SortingOrderManager.Instance.RemoveSortingOnLayerObject(this);
    }

    private void OnDisable()
    {
        RemoveSelfFromSortingOrderManager();
    }

    public string Id => id;
    public bool State => _isDead;


    public SpriteRenderer SpriteRenderer => spriteRenderer;

    public float YCoordinate => transform.position.y;

}

using System.Collections.Generic;
using UnityEngine;

public class SortingOrderManager : MonoBehaviour
{
    public static SortingOrderManager Instance { get; private set; }
    private List<ISortingOnLayerObject> _sortingOnLayerObjects = new();

    private void Awake()
    {
        if(!Instance) {Instance = this; DontDestroyOnLoad(gameObject);}
        else Destroy(gameObject);
    }

    public void AddSortingOnLayerObject(ISortingOnLayerObject sortingOnLayerObject)
    {
        if(_sortingOnLayerObjects.Contains(sortingOnLayerObject)) return;
        _sortingOnLayerObjects.Add(sortingOnLayerObject);
    }

    public void RemoveSortingOnLayerObject(ISortingOnLayerObject sortingOnLayerObject)
    {
        if(!_sortingOnLayerObjects.Contains(sortingOnLayerObject)) return;
        _sortingOnLayerObjects.Remove(sortingOnLayerObject);
    }

    private void LateUpdate()
    {
        foreach (var srObj in _sortingOnLayerObjects)
        {
            if (!srObj.SpriteRenderer) continue;
            srObj.SpriteRenderer.sortingOrder = Mathf.RoundToInt(-srObj.YCoordinate * 10);
        }
    }
}

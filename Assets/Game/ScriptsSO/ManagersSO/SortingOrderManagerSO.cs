using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SortingOrderManager", menuName = "ManagersSO/SortingOrderManager")]
public class SortingOrderManagerSO : ScriptableObject
{
    private List<ISortingOnLayerObject> _sortingOnLayerObjects = new();


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

    private IEnumerator SortObjectsRoutine()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            foreach (var srObj in _sortingOnLayerObjects)
            {
                if (!srObj.SpriteRenderer) continue;
                srObj.SpriteRenderer.sortingOrder = Mathf.RoundToInt(-srObj.YCoordinate * 10);
            }
        }
    }
}

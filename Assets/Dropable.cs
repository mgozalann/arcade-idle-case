using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Dropable : MonoBehaviour
{
    public List<Item> DroppedItems = new List<Item>();

    [SerializeField] private Transform[] _dropPoints;
    
    [SerializeField] private float _offSetY;
    [SerializeField] private int _maxCount;
    public Item DropableItem;

    [SerializeField] private Transform _parent;
    public void TakeItem(Item item)
    {
        if (DroppedItems.Count < _maxCount)
        {
            SetPosition(item);
        }
    }

    private void SetPosition(Item item)
    {
        item.transform.SetParent(null);
        
        int dropPointIndex = DroppedItems.Count % _dropPoints.Length;
        float yIncrease = (DroppedItems.Count / _dropPoints.Length) * _offSetY;
        
        Vector3 targetPos = _dropPoints[dropPointIndex].position + new Vector3(0, yIncrease, 0);

        item.transform.DOLocalJump(targetPos, 2f, 1, .25f).SetEase(Ease.InOutQuad).
            OnComplete(() =>DroppedItems.Add(item));
        item.transform.DOLocalRotate(Vector3.zero, .25f).SetEase(Ease.InSine);
    }
}
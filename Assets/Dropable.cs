using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Dropable : MonoBehaviour
{
    private List<Item> _droppedItems = new List<Item>();

    [SerializeField] private Transform[] _dropPoints;

    [SerializeField] private float _offSetY;
    [SerializeField] private int _maxCount;

    [SerializeField] private Item _dropableItem;
    public Item DropableItem => _dropableItem;

    [SerializeField] private Transformer _transformer;

    private float _timer;
    [SerializeField] private float _spawnInterval;
    
    private void Start()
    {
        _timer = _spawnInterval;
        StartCoroutine(SpawnItemRoutine());
    }

    public void TakeItem(Item item)
    {
        if (_droppedItems.Count < _maxCount)
        {
            SetPosition(item);
        }
    }
    
    private void SetPosition(Item item)
    {
        item.transform.SetParent(null);

        int dropPointIndex = _droppedItems.Count % _dropPoints.Length;
        float yIncrease = (_droppedItems.Count / _dropPoints.Length) * _offSetY;

        Vector3 targetPos = _dropPoints[dropPointIndex].position + new Vector3(0, yIncrease, 0);

        item.transform.DOLocalJump(targetPos, 2f, 1, .25f).SetEase(Ease.InOutQuad);
        item.transform.DOLocalRotate(Vector3.zero, .25f).SetEase(Ease.InSine);

        _droppedItems.Add(item);
    }

    private IEnumerator SpawnItemRoutine()
    {
        while (true)
        {
            if (_transformer.Collectable.SpawnedItems.Count >= _transformer.MaxItemCount && _droppedItems.Count <= 0)
            {
                yield return null;
            }
            else
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                {
                    if (_droppedItems.Count > 0)
                    {
                        
                        Vector3 targetPos = _transformer.transform.position + new Vector3(0, .5f, 0);

                        Item item = _droppedItems[^1];

                        _droppedItems.Remove(item);
                        
                        item.transform.DOLocalJump(targetPos, 2f, 1, .25f).SetEase(Ease.InOutQuad);
                        item.transform.DOLocalRotate(Vector3.zero, .25f).SetEase(Ease.InSine);

                        yield return new WaitForSeconds(.25f);
                        
                        ItemObjectPoolManager.Instance.ReturnObject(item.Type,item);
                        
                        _transformer.SpawnObject();
                        
                        _timer = _spawnInterval;
                    }
                }
            }
            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
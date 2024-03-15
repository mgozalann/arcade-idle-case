using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    
    [SerializeField] private Collectable _collectable;
    public Collectable Collectable => _collectable;

    [SerializeField] private float _offSetY;

    [SerializeField] private Item _transformedItem;
    public int MaxItemCount { get; private set; }
    public void SpawnObject()
    {
        int spawnPointIndex = _collectable.SpawnedItems.Count % _spawnPoints.Length;
        float yIncrease = (_collectable.SpawnedItems.Count / _spawnPoints.Length) * _offSetY;

        Item instance = ItemObjectPoolManager.Instance.GetObject(_transformedItem.Type);
        instance.transform.position = this.transform.position;
        instance.transform.rotation = Quaternion.identity;

        Vector3 targetPos = _spawnPoints[spawnPointIndex].position + new Vector3(0, yIncrease, 0);
        
        instance.transform.DOJump(targetPos, 2f, 1, .5f).SetEase(Ease.InOutQuad).
            OnComplete(() => _collectable.SpawnedItems.Add(instance));
    }
}
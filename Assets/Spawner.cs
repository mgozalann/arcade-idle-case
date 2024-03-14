using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Item _itemPrefab;
    [SerializeField] private Collectable _collectable;
    
    [SerializeField] private float _offSetY;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _maxItemCount;

    private void Start()
    {
        StartCoroutine(SpawnObjectsCoroutine());
    }

    private IEnumerator SpawnObjectsCoroutine()
    {
        while (true)
        {
            if (_collectable.SpawnedItems.Count < _maxItemCount)
            {
                
                int spawnPointIndex = _collectable.SpawnedItems.Count % _spawnPoints.Length;
                float yIncrease = (_collectable.SpawnedItems.Count / _spawnPoints.Length) * _offSetY;
                
                Item item = Instantiate(_itemPrefab, transform.position, Quaternion.identity);

                Vector3 targetPos = _spawnPoints[spawnPointIndex].position + new Vector3(0, yIncrease, 0);
            
                item.transform.DOJump(targetPos, 2f, 1, .25f).SetEase(Ease.InOutQuad);
                
                yield return new WaitForSeconds(.25f);
                
                _collectable.SpawnedItems.Add(item);
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
        // ReSharper disable once IteratorNeverReturns
    }
    
    // private void Start()
    // {
    //     _timer = _spawnInterval;
    // }
    //
    // void Update()
    // {
    //     if (_collectable.SpawnedItems.Count >= _maxItemCount) return;
    //
    //     _timer -= Time.deltaTime;
    //
    //     if (_timer <= 0)
    //     {
    //         SpawnObject();
    //         _timer = _spawnInterval;
    //     }
    // }

    // private void SpawnObject()
    // {
    //     int spawnPointIndex = _collectable.SpawnedItems.Count % _spawnPoints.Length;
    //     float yIncrease = (_collectable.SpawnedItems.Count / _spawnPoints.Length) * _offSetY;
    //         
    //     Item item = Instantiate(_itemPrefab, transform.position, Quaternion.identity);
    //
    //     Vector3 targetPos = _spawnPoints[spawnPointIndex].position + new Vector3(0, yIncrease, 0);
    //     
    //     item.transform.DOJump(targetPos, 2f, 1, .5f).SetEase(Ease.InOutQuad).
    //         OnComplete(() => _collectable.SpawnedItems.Add(item));
    // }
}
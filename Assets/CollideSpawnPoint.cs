using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollideSpawnPoint : MonoBehaviour
{
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private Transform _storage;

    [SerializeField] private float _collectTime; 
    [SerializeField] private float _deliverTime;
    [SerializeField] private int _maxItemCount;
    
    private float _timer;

    private Vector3 _lastItemPos;

    [SerializeField] private List<Item> _inventory = new List<Item>();

    private void Start()
    {
        _lastItemPos = Vector3.zero;

        _timer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Collectable collectable))
        {
            OnCollectableArea(collectable);
        }

        if (other.TryGetComponent(out Garbage garbage))
        {
            //_onEnterGarbageZone();
        }

        if (other.TryGetComponent(out Dropable dropable))
        {
            DepositeItem(dropable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Collectable collectable))
        {
            _timer = 0f;
        }
    }

    private void GetItem(Item item)
    {
        ResetLastPosition();

        item.transform.SetParent(_storage);

        item.transform.DOLocalJump(_lastItemPos + _offSet, 2f, 1, .25f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                _lastItemPos = item.transform.localPosition;

                _inventory.Add(item);
            });

        item.transform.DOLocalRotate(Vector3.zero, .25f).SetEase(Ease.InSine);

    }


    private bool CheckMaxItemCount()
    {
        bool value = _inventory.Count < _maxItemCount;

        return value;
    }

    private bool _isEmpty()
    {
        bool value = _inventory.Count == 0;

        return value;
    }

    private void OnCollectableArea(Collectable collectable)
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= _collectTime)
        {
            Item instance = collectable.GetItem();

            if (instance == null) return;

            if (CheckMaxItemCount())
            {
                GetItem(instance);

                _timer = 0f;
            }
        }
    }

    private void OnGarbageArea()
    {
        _timer += Time.fixedDeltaTime;
        if (_isEmpty())
        {
            return;
        }

        if (_timer >= _deliverTime)
        {
            RemoteItem();

            _timer = 0f;

            return;
        }
    }
    private void RemoteItem()
    {
        Item item = _inventory[CheckInventory()];
        _inventory.RemoveAt(CheckInventory());

        ResetLastPosition();

        Destroy(item.gameObject);
    }

    private void DepositeItem(Dropable dropable)
    {
        if (_isEmpty())
        {
            return;
        }

        _timer += Time.fixedDeltaTime;
        if (_timer >= _deliverTime)
        {
            for (int i = _inventory.Count - 1; i >= 0; i--)
            {
                if (_inventory[i].Type == dropable.DropableItem.Type)
                {
                    dropable.TakeItem(_inventory[i]);
                    _inventory.RemoveAt(i);

                    ResetPositions();
                    ResetLastPosition();

                    _timer = 0;
                    break;
                }
            }
        }
    }

    private int CheckInventory()
    {
        int id = 0;

        if (_inventory.Count - 1 > 0)
        {
            id = _inventory.Count - 1;
        }

        return id;
    }

    private void ResetPositions()
    {
        if (_inventory.Count > 0)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                _inventory[i].transform.localPosition = i * _offSet;
            }
            _lastItemPos = _inventory[^1].transform.localPosition;
        }
    }

    private void ResetLastPosition()
    {
        if (_inventory.Count == 0)
        {
            _lastItemPos = Vector3.zero;
        }
    }
}
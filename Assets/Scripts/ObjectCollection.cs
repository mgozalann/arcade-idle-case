using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class ObjectCollection : MonoBehaviour
{
    public event Action OnInventoryFull; 
    public event Action OnInventoryEmpty; 

    [SerializeField] private List<Item> _inventory = new List<Item>();
    
    [SerializeField] private Vector3 _offSet;
    [SerializeField] private Transform _storage;

    [SerializeField] private float _collectTime; 
    [SerializeField] private float _deliverTime;
    [SerializeField] private int _maxItemCount;

    private float _timer;
    private Vector3 _lastItemPos;

    private void Start()
    {
        ResetLastPosition();

        _timer = 0f;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Collectable collectable))
        {
            CollectItem(collectable);
        }

        if (other.TryGetComponent(out Garbage garbage))
        {
            OnGarbageArea(garbage);
        }

        if (other.TryGetComponent(out Dropable dropable))
        {
            DepositeItem(dropable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _timer = 0f;
    }

    private void GetItem(Item item)
    {
        ResetLastPosition();

        item.transform.SetParent(_storage);

        Vector3 pos = _lastItemPos + _offSet;

        item.transform.DOLocalJump(pos, 1.75f, 1, .25f).SetEase(Ease.InOutQuad);
        item.transform.DOLocalRotate(Vector3.zero, .25f).SetEase(Ease.InSine);

        _lastItemPos = pos;
        _inventory.Add(item);
        CheckMaxItemCount();

    }

    private void CollectItem(Collectable collectable)
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= _collectTime)
        {
            _timer = 0f;

            if (CheckMaxItemCount())
            {
                Item instance = collectable.GetItem();

                if (instance == null) return;

                GetItem(instance);
            }
        }
    }

    private void OnGarbageArea(Garbage garbage)
    {
        _timer += Time.fixedDeltaTime;
        if (_isEmpty())
        {
            return;
        }

        if (_timer >= _deliverTime)
        {
            RemoteItem(garbage);

            _timer = 0f;

            return;
        }
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
    private void RemoteItem(Garbage garbage)
    {
        Item item = _inventory[CheckInventory()];
        _inventory.RemoveAt(CheckInventory());

        ResetLastPosition();
        
        garbage.TakeItem(item);
    }

    private bool CheckMaxItemCount()
    {
        bool value = _inventory.Count < _maxItemCount;

        if (!value)
        {
            OnInventoryFull?.Invoke();
        }
        
        return value;
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
    
    private bool _isEmpty()
    {
        bool value = _inventory.Count == 0;

        if (value)
        {
            OnInventoryEmpty?.Invoke();
        }
        
        return value;
    }
}
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class CollideSpawnPoint : MonoBehaviour
{
    [SerializeField] private Vector3 _offSet;
    private Vector3 _lastItemPos;

    [SerializeField] private float _timeToCollect; //isim değişecek
    [SerializeField] private float _timeToDeliver; //isim değişecek

    [SerializeField] private float _timer;

    [SerializeField] private int _maxItemStorage; //isim değişecek
    private int _currentStorage; //isim değişecek

    [SerializeField] private Transform _storage;

    private List<Item> _inventory = new List<Item>();
    public List<Item> Inventory => _inventory;

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

        //
        //     case PlayerPrefKeys.TranformerInput:
        //         if(other.TryGetComponent(out TransformerInputStorage transformetInputStorage))
        //         {
        //             _onEnterTransformerInput(transformetInputStorage);
        //         }
        //     break;
        //
        //     case PlayerPrefKeys.TransformerStorage:
        //         if (other.TryGetComponent(out TransformerStorage tranformerStorage))
        //         {
        //             _onEnterTransformerZone(tranformerStorage);
        //         }
        //     break;
        //
        // }
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

                _currentStorage++;
            });

        item.transform.DOLocalRotate(Vector3.zero, .25f).SetEase(Ease.InSine);
    }


    private bool _checkMaxItemCount()
    {
        bool value = false;

        if (_currentStorage < _maxItemStorage)
        {
            value = true;
        }

        else
        {
            // if (!_isPlayer)
            //     EventSystem.CallInvetoryFull();
        }

        return value;
    }

    //
    private bool _isEmpty()
    {
        bool value = _currentStorage == 0;

        return value;
    }

    private void OnCollectableArea(Collectable collectable)
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= _timeToCollect)
        {
            Item instance = collectable.GetItem();

            if(instance == null) return;
            
            if (_checkMaxItemCount())
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

        if (_timer >= _timeToDeliver)
        {
            _remoteItem();

            _timer = 0f;

            return;
        }
    }

    // private void _onEnterTransformerInput(TransformerInputStorage transformerInputStorage)
    // {
    //     _timer += Time.fixedDeltaTime;
    //     if (_isEmpty())
    //     {
    //         return;
    //     }
    //
    //     if (transformerInputStorage.IsFull())
    //     {
    //         return;
    //     }
    //
    //     if (_timer >= _timeToDeliver)
    //     {
    //        /7- DepositeItem(transformerInputStorage);
    //         _timer = 0f;
    //
    //         return;
    //     }
    // }

    // private void _onEnterTransformerZone(TransformerStorage storage)
    // {
    //     _timer += Time.fixedDeltaTime;
    //     if (_timer < _timeToCollect)
    //     {
    //         return;
    //     }
    //
    //     if (storage.IsEmpty())
    //     {
    //         return;
    //     }
    //
    //     if (_checkMaxItemCount())
    //     {
    //         _getItem(storage.GetItem());
    //
    //         _timer = 0f;
    //     }
    // }

    private void _remoteItem()
    {
        Item item = _inventory[_checkInventory()];
        _inventory.RemoveAt(_checkInventory());
    
        ResetLastPosition();
    
        _currentStorage--;
    
        Destroy(item.gameObject);
    }

    private void DepositeItem(Dropable dropable)
    {
        if (_isEmpty())
        {
            return;
        }
        
        _timer += Time.fixedDeltaTime;
        if (_timer >= _timeToDeliver)
        {
            for (int i = _inventory.Count - 1; i >= 0; i--)
            {
                if (_inventory[i].Type == dropable.DropableItem.Type)
                {
                    dropable.TakeItem(_inventory[i]);
                    _inventory.RemoveAt(i);
                    
                    ResetLastPosition();
                    _currentStorage--;

                    
                    _timer = 0;
                    break;
                }
            }
        }
    }

    private int _checkInventory()
    {
    
        int id = 0;
    
        if (_inventory.Count - 1 > 0)
        {
            id = _inventory.Count - 1;
        }
    
        return id;
    }

    private void ResetLastPosition()
    {
        if (_currentStorage == 0)
        {
            _lastItemPos = Vector3.zero;
        }
    }
}
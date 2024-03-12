using UnityEngine;



public class CollideSpawnPoint : MonoBehaviour
{
    [SerializeField] private Vector3 _offSet;
    private Vector3 _lastItemPos;
    
    [SerializeField] private float _timeToCollect; //isim değişecek
    [SerializeField] private float _timeToDeliver;  //isim değişecek
    
    private float _timer;
    
    [SerializeField] private int _maxItemStorage; //isim değişecek
    private int _currentStorage; //isim değişecek
    
    [SerializeField] private Transform _storage;

   // private List<Item> _inventory = new List<Item>();
   
   private void Start()
   {
       _lastItemPos = Vector3.zero;

       _timer = 0f;
   }

   private void OnTriggerStay(Collider other)
    {

        if (other.TryGetComponent(out Spawner spawner))
        {
          _onEnterSpawnerZone(spawner);
        }
        
        
        
        // switch (other.tag)
        // {
        //     case PlayerPrefKeys.GarbageZone:
        //         _onEnterGarbageZone();
        //     break;
        //
        //     case PlayerPrefKeys.ItemSpawner:
        //         if (other.TryGetComponent(out SpawnerMachineStorage storage))
        //         {
        //             _onEnterSpawnerZone(storage);
        //         }
        //     break;
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

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag(PlayerPrefKeys.ItemSpawner))
    //     {
    //         _timer = 0f;
    //     }
    // }

    private void _getItem(Item item)
    {
        _resetLastPosition();
    
        item.transform.SetParent(_storage);
    
        item.transform.localPosition = Vector3.zero;
        item.transform.localPosition = _lastItemPos + _offSet;
        item.transform.localRotation = Quaternion.Euler(Vector3.zero);
    
        _lastItemPos = item.transform.localPosition;
    
        //_inventory.Add(item);
    
        _currentStorage++;
    }
    //
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
    // private bool _isEmpty()
    // {
    //     bool value = false;
    //
    //     if (_currentStorage == 0)
    //     {
    //         value = true;
    //
    //         if(!_isPlayer)
    //             EventSystem.CallInventoryEmpty();
    //     }
    //
    //     return value;
    // }
    //
    private void _onEnterSpawnerZone(Spawner spawner)
    {
        _timer += Time.fixedDeltaTime;
        if (_timer < _timeToCollect)
        {
            return;
        }
    
        // if (storage.IsEmpty())
        // {
        //     return;
        // }
    
        if (_checkMaxItemCount())
        {
            //_getItem(storage.GetItem());
    
            _timer = 0f;
        }
    }

    // private void _onEnterGarbageZone()
    // {
    //     _timer += Time.fixedDeltaTime;
    //     if (_isEmpty())
    //     {
    //         return;
    //     }
    //
    //     if (_timer >= _timeToDeliver)
    //     {
    //         _remoteItem();
    //
    //         _timer = 0f;
    //
    //         return;
    //     }
    // }

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

    // private void _remoteItem()
    // {
    //     Item item = _inventory[_checkInventory()];
    //     _inventory.RemoveAt(_checkInventory());
    //
    //     _resetLastPosition();
    //
    //     _currentStorage--;
    //
    //     Destroy(item.gameObject);
    // }

    // public void DepositeItem(TransformerInputStorage transformerInputStorage)
    // {
    //     if (_isEmpty())
    //     {
    //         return;
    //     }
    //
    //     Item itemToDeliver = _inventory[_checkInventory()];
    //
    //     if (itemToDeliver.Type == transformerInputStorage.requiredItem)
    //     {
    //         _inventory.Remove(itemToDeliver);
    //         transformerInputStorage.SetItemPosition(itemToDeliver);
    //
    //         _resetLastPosition();
    //
    //         _currentStorage--;
    //     }
    //
    //     else
    //     {
    //         return;
    //     }
    // }

    // private int _checkInventory()
    // {
    //
    //     int id = 0;
    //
    //     if (_inventory.Count - 1 > 0)
    //     {
    //         id = _inventory.Count - 1;
    //     }
    //
    //     return id;
    // }

    private void _resetLastPosition()
    {
        if (_currentStorage == 0)
        {
            _lastItemPos = Vector3.zero;
        }
    }
}
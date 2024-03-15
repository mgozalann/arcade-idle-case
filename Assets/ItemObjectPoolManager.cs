using UnityEngine;

public class ItemObjectPoolManager : MonoBehaviour
{
    private static ItemObjectPoolManager _instance;
    public static ItemObjectPoolManager Instance => _instance;
    
    [SerializeField] private ItemObjectPool[] _itemObjectPools;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        foreach (var itemObjectPool in _itemObjectPools)
        {
            GameObject poolObject = new GameObject(itemObjectPool.ItemType.ToString() + "ObjectPool");
            ItemObjectPool pool = poolObject.AddComponent<ItemObjectPool>();
            pool.Initialize(itemObjectPool.Prefab, itemObjectPool.ItemType, itemObjectPool.PoolSize);
        }
    }

    public Item GetObject(ItemType type)
    {
        foreach (var itemObjectPool in _itemObjectPools)
        {
            if (itemObjectPool.ItemType == type)
                return itemObjectPool.GetObject();
        }

        return null;
    }

    public void ReturnObject(ItemType type, Item obj)
    {
        foreach (var itemObjectPool in _itemObjectPools)
        {
            if (itemObjectPool.ItemType == type)
            {
                itemObjectPool.ReturnObject(obj);
                return;
            }
        }
    }
}
    using System.Collections.Generic;
    using UnityEngine;

    public class ItemObjectPool : MonoBehaviour
    {
        public Item Prefab; // Havuzdaki nesnelerin prototipi
        public ItemType ItemType; // Nesnelerin türü
        public int PoolSize; // Havuzdaki nesne sayısı

        private Queue<Item> _pool = new Queue<Item>();

        public void Initialize(Item prefab, ItemType type, int poolSize)
        {
            Prefab = prefab;
            ItemType = type;
            PoolSize = poolSize;

            // Nesneleri havuza ekle
            for (int i = 0; i < PoolSize; i++)
            {
                Item obj = Instantiate(Prefab, transform);
                obj.gameObject.SetActive(false);
                obj.Type = ItemType;
                _pool.Enqueue(obj);
            }
        }

        // Havuzdan nesne al
        public Item GetObject()
        {
            if (_pool.Count > 0)
            {
                Item obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                Item obj = Instantiate(Prefab, transform);
                obj.Type = ItemType;
                return obj;
            }
        }

        // Nesneyi havuza geri döndür
        public void ReturnObject(Item obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            _pool.Enqueue(obj);
        }
    }
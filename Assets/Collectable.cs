using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public List<Item> SpawnedItems = new List<Item>();

    public Item GetItem()
    {
        if (SpawnedItems.Count <= 0) return null;

        Item item = SpawnedItems[^1];
        
        SpawnedItems.RemoveAt(SpawnedItems.Count -1);
        
        return item;
    }
}
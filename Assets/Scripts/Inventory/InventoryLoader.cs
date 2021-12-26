using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLoader : MonoBehaviour
{
    [SerializeField]
    private InventoryController _inventory;

    [SerializeField]
    private List<Item> _prefabItems;

    void Start()
    {
        _prefabItems.ForEach(p => _inventory.PutItemPrefab(p));
    }
}

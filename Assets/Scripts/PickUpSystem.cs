using System.Collections;
using System.Collections.Generic;
using Inventory.Data;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] InventoryData inventoryData;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<ItemPickUp>(out var item))
        {
            var remainingItemQuantity = inventoryData.AddItem(item.ItemData, item.ItemQuantity);
            if (remainingItemQuantity == 0)
                item.OnPickUp();
            else
                item.ItemQuantity = remainingItemQuantity;
        }
    }
}

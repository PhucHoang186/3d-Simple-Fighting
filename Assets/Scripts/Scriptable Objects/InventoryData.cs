using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : ScriptableObject
{
    public List<InventoryItem> inventoryItems;
    public int Size { get; private set; } = 10;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemData itemData, int quantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = new InventoryItem
                {
                    itemData = itemData,
                    quantity = quantity,
                };
            }
        }
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        var returnValues = new Dictionary<int , InventoryItem>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].IsEmpty)
                continue;
            returnValues[i] = inventoryItems[i];
        }
        return returnValues;
    }

}

[Serializable]
public struct InventoryItem
{
    public int quantity;
    public ItemData itemData;

    public bool IsEmpty => itemData == null;
    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            itemData = this.itemData,
            quantity = newQuantity,
        };
    }

    public static InventoryItem GetEmptyItem()
    => new InventoryItem
    {
        itemData = null,
        quantity = 0,
    };
}

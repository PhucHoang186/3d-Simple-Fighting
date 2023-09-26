using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Data
{
    [CreateAssetMenu(menuName = "Inventory Data")]
    public class InventoryData : ScriptableObject
    {
        [SerializeField] private List<InventoryItem> inventoryItems;
        [field: SerializeField]
        public int Size { get; private set; } = 10;
        public Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemData itemData, int quantity)
        {
            if (!itemData.IsStackable)
            {
                quantity = AddNonStackableItem(itemData, 1);
            }
            else
            {
                quantity = AddStackableItem(itemData, quantity);
                quantity = AddMultipleItemsToEmptySlots(itemData, quantity);
            }
            InformAboutInventoryChange();
            return quantity;
        }

        private bool IsInventoryFull()
        {
            foreach (var item in inventoryItems)
            {
                if (item.IsEmpty)
                    return false;
            }
            return true;
        }

        private int AddNonStackableItem(ItemData itemData, int quantity = 1)
        {
            return AddItemToFirstEmptySlot(itemData, quantity);
        }


        private int AddStackableItem(ItemData itemData, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                var checkItemData = inventoryItems[i];
                if (!checkItemData.IsEmpty && checkItemData.itemData.ID == itemData.ID)
                {
                    var newQuantity = quantity + checkItemData.quantity; // example max size 10 , new quantity 14
                    var itemsLeftToAdd = newQuantity - checkItemData.itemData.MaxStackSize; // itemsLeftToAdd = 4

                    if (itemsLeftToAdd > 0)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].itemData.MaxStackSize);
                        quantity = itemsLeftToAdd;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(newQuantity);
                        return 0;
                    }
                }
            }
            return quantity;
        }

        private int AddItemToFirstEmptySlot(ItemData itemData, int quantity)
        {
            InventoryItem newItem = new()
            {
                itemData = itemData,
                quantity = quantity,
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return 0;
                }
            }
            return -1;
        }

        private int AddMultipleItemsToEmptySlots(ItemData itemData, int quantity)
        {
            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, itemData.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstEmptySlot(itemData, newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItem inventoryItem)
        {
            AddItem(inventoryItem.itemData, inventoryItem.quantity);
        }

        public InventoryItem GetItemAtIndex(int itemIndex)
        {
            if (itemIndex >= inventoryItems.Count)
                return InventoryItem.GetEmptyItem();
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            (inventoryItems[itemIndex_1], inventoryItems[itemIndex_2]) = (inventoryItems[itemIndex_2], inventoryItems[itemIndex_1]);
            InformAboutInventoryChange();
        }

        private void InformAboutInventoryChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            var returnValues = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
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
        => new()
        {
            itemData = null,
            quantity = 0,
        };
    }
}
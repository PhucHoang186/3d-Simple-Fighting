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

        public int AddItem(ItemData itemData, int quantity, List<ItemParameter> itemState = null)
        {
            if (!itemData.IsStackable)
            {
                quantity = AddNonStackableItem(itemData, 1, itemState);
            }
            else
            {
                quantity = AddStackableItem(itemData, quantity);
                quantity = AddMultipleItemsToEmptySlots(itemData, quantity, itemState);
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

        private int AddNonStackableItem(ItemData itemData, int quantity = 1, List<ItemParameter> itemState = null)
        {
            return AddItemToFirstEmptySlot(itemData, quantity, itemState);
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

        private int AddItemToFirstEmptySlot(ItemData itemData, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new()
            {
                itemData = itemData,
                quantity = quantity,
                itemState = new List<ItemParameter>(itemState ?? itemData.DefaultParameterList)
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

        private int AddMultipleItemsToEmptySlots(ItemData itemData, int quantity, List<ItemParameter> itemState = null)
        {
            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, itemData.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstEmptySlot(itemData, newQuantity, itemState);
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

        public void RemoveItem(int itemIndex, int quantity)
        {
            if (inventoryItems.Count <= itemIndex || inventoryItems[itemIndex].IsEmpty)
                return;

            var remainingQuantity = inventoryItems[itemIndex].quantity - quantity;
            if (remainingQuantity > 0)
            {
                inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(remainingQuantity);
            }
            else
            {
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            }
            InformAboutInventoryChange();
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemData itemData;
        public List<ItemParameter> itemState;

        public bool IsEmpty => itemData == null;
        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                itemData = this.itemData,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }

        public static InventoryItem GetEmptyItem()
        => new()
        {
            itemData = null,
            quantity = 0,
            itemState = new List<ItemParameter>()
        };
    }
}
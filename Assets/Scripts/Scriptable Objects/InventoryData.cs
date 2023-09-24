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
                while (quantity > 0 && !IsInventoryFull())
                {
                    quantity -= AddNonStackableItem(itemData, 1);
                }
            }
            else
            {
                quantity = AddStackableItem(itemData, quantity);
            }
            InformAboutInventoryChange();
            return quantity;
        }

        private bool IsInventoryFull()
        {
            foreach (var item in inventoryItems)
            {
                if (!item.IsEmpty)
                    return false;
            }
            return true;
        }

        private int AddNonStackableItem(ItemData itemData, int quantity = 1)
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
                return quantity;
            }
            return 0;
        }

        private int AddStackableItem(ItemData itemData, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                var checkItemData = inventoryItems[i];
                if (!checkItemData.IsEmpty && checkItemData.itemData.ID == itemData.ID)
                {
                    var newQuantity = quantity + checkItemData.quantity;
                    var itemsLeftToAdd = newQuantity - checkItemData.itemData.MaxStackSize;

                    if (itemsLeftToAdd > 0)
                    {
                        inventoryItems[i].ChangeQuantity(checkItemData.itemData.MaxStackSize);
                        quantity = itemsLeftToAdd;
                    }
                    else
                    {
                        inventoryItems[i].ChangeQuantity(newQuantity);
                        return 0;
                    }

                }
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
            InventoryItem tempItem = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = tempItem;
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
        => new InventoryItem
        {
            itemData = null,
            quantity = 0,
        };
    }
}
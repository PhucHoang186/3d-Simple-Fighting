using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Inventory.Data;
using Inventory.UI;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] bool openOnPlay;
        [SerializeField] UIInventoryTab inventoryTab;
        [SerializeField] InventoryData inventoryData;
        [SerializeField] List<InventoryItem> initialItemList = new();

        private void Start()
        {
            PrepareUI();
            PrepareInvetoryData();

            if (openOnPlay)
                inventoryTab.Show();
            else
                inventoryTab.Hide();
        }

        private void PrepareInvetoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (var invenToryItem in initialItemList)
            {
                if (invenToryItem.IsEmpty)
                    continue;
                inventoryData.AddItem(invenToryItem);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> dictionary)
        {
            inventoryTab.ResetAllItems();
            foreach (var item in dictionary)
            {
                inventoryTab.UpdateData(item.Key, item.Value.itemData.ItemIcon, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryTab.InitInventoryItemsUI(inventoryData.Size);
            inventoryTab.OnDescriptionRequested += OnDescriptionRequested;
            inventoryTab.OnItemActionsRequested += OnItemActionsRequested;
            inventoryTab.OnStartDragging += OnStartDragging;
            inventoryTab.OnSwapItems += OnSwapItems;
        }

        private void OnSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void OnStartDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAtIndex(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryTab.CreateDragItem(inventoryItem.itemData.ItemIcon, inventoryItem.quantity);
        }

        private void OnItemActionsRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAtIndex(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            IDestroyableItem destroyableItem = inventoryItem.itemData as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }
            IItemAction itemAttion = inventoryItem.itemData as IItemAction;
            if (inventoryItem.itemData != null)
            {
                itemAttion.PerformAction(gameObject, inventoryItem.itemState);
            }

        }

        private void OnDescriptionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAtIndex(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryTab.ResetSelection();
                return;
            }
            string description = PrepareDescription(inventoryItem);
            inventoryTab.UpdateDescription(itemIndex, inventoryItem.itemData.ItemIcon, inventoryItem.itemData.ItemName, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new();
            sb.Append(inventoryItem.itemData.ItemDescription);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].parameterData.ParameterName}" +
                $":{inventoryItem.itemState[i].value}" +
                $"/{inventoryItem.itemData.DefaultParameterList[i].value}");
            }

            return sb.ToString();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!inventoryTab.isActiveAndEnabled)
                {
                    inventoryTab.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryTab.UpdateData(item.Key, item.Value.itemData.ItemIcon, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryTab.Hide();
                }
            }
        }
    }
}
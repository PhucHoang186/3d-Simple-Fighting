using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryTab : MonoBehaviour
    {
        [SerializeField] UIItem itemPrefab;
        [SerializeField] Transform contentPanel;
        [SerializeField] UiInventoryDescription uiInventoryDescription;
        [SerializeField] MouseFollower mouseFollower;
        private List<UIItem> inventoryUIList = new();
        public event Action<int> OnDescriptionRequested, OnItemActionsRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;
        private int currentSelectedIndex = -1;

        void Start()
        {
            mouseFollower.Toggle(false);
        }

        public void InitInventoryItemsUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                var itemUI = Instantiate(itemPrefab, contentPanel);
                itemUI.transform.ResetTransform();
                inventoryUIList.Add(itemUI);
                itemUI.SetUpActions(HandleClickedItem, HandleRightClickedItem, HandleSwapItem, HandleBeginDragItem, HandleEndDragItem);
            }
        }

        private void HandleShowActionsItem(UIItem item)
        {

        }

        private void HandleEndDragItem(UIItem item)
        {
            ResetDragItem();
        }

        private void HandleBeginDragItem(UIItem item)
        {
            int index = inventoryUIList.IndexOf(item);
            if (index == -1)
                return;
            currentSelectedIndex = index;
            OnStartDragging?.Invoke(index);
        }

        public void CreateDragItem(Sprite itemSprite, int itemQuantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(itemSprite, itemQuantity);
        }

        private void HandleSwapItem(UIItem item)
        {
            int index = inventoryUIList.IndexOf(item);
            if (index == -1)
                return;
            OnSwapItems?.Invoke(currentSelectedIndex, index);
        }

        private void HandleClickedItem(UIItem item)
        {
            int index = inventoryUIList.IndexOf(item);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        private void HandleRightClickedItem(UIItem item)
        {
            int index = inventoryUIList.IndexOf(item);
            if (index == -1)
                return;
            OnItemActionsRequested?.Invoke(index);
        }

        private void ResetDragItem()
        {
            mouseFollower.Toggle(false);
            currentSelectedIndex = -1;
        }

        public void Show()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDragItem();
        }

        public void ResetSelection()
        {
            uiInventoryDescription.ResetDescription();
            DeSelectAllItems();
        }

        private void DeSelectAllItems()
        {
            foreach (var item in inventoryUIList)
            {
                item.UnSelect();
            }
        }

        private void ResetAllItemDatas()
        {
            foreach (var item in inventoryUIList)
            {
                item.ResetData();
            }
        }

        public void UpdateData(int itemIndex, Sprite itemSprite, int itemQuantity)
        {
            if (itemIndex < inventoryUIList.Count)
            {
                inventoryUIList[itemIndex].SetData(itemSprite, itemQuantity);
            }
        }

        internal void UpdateDescription(int itemIndex, Sprite itemIcon, string itemName, string itemDesciption)
        {
            uiInventoryDescription.SetDescription(itemIcon, itemName, itemDesciption);
            DeSelectAllItems();
            inventoryUIList[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            ResetAllItemDatas();
            DeSelectAllItems();
        }
    }
}
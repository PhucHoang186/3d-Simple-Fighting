using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventoryTab : MonoBehaviour
{
    [SerializeField] UIItem itemPrefab;
    [SerializeField] Transform contentPanel;
    [SerializeField] UiInventoryDescription uiInventoryDescription;
    [SerializeField] MouseFollower mouseFollower;
    private List<UIItem> inventoryUIList = new();

    [Header("testing")]
    [SerializeField] Sprite testItemSprite;
    [SerializeField] Sprite testItemSprite2;
    [SerializeField] string testItemTitle;
    [SerializeField] string testItemDescription;
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
        CreateDragItem(item);
    }

    private void CreateDragItem(UIItem item)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(item.ItemSprite, item.ItemQuantity);
    }

    private void HandleSwapItem(UIItem item)
    {
        int index = inventoryUIList.IndexOf(item);
        if (index == -1)
            return;
        // var currentItem = inventoryUIList[index];
        // var itemToSwap = inventoryUIList[currentSelectedIndex];
        // UIItem tempItem = new UIItem();
        // tempItem.ItemSprite = itemToSwap.ItemSprite;
        // tempItem.ItemQuantity = itemToSwap.ItemQuantity;


        // itemToSwap.SetData(currentItem.ItemSprite, currentItem.ItemQuantity);
        // currentItem.SetData(tempItem.ItemSprite, tempItem.ItemQuantity);
        // currentSelectedIndex = -1;

        OnSwapItems?.Invoke(currentSelectedIndex, index);
    }

    private void HandleClickedItem(UIItem item)
    {
        HandleSelectedItem(item);
    }

    private void HandleSelectedItem(UIItem item)
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
        gameObject.SetActive(true);
        ResetSelection();

    }

    private void ResetSelection()
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

    public void Hide()
    {
        gameObject.SetActive(false);
        ResetDragItem();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryTab : MonoBehaviour
{
    [SerializeField] UIItem itemPrefab;
    [SerializeField] Transform contentPanel;
    [SerializeField] UiInventoryDescription uiInventoryDescription;
    private List<UIItem> inventoryUIList = new();

    [Header("testing")]
    [SerializeField] Sprite testItemSprite;
    [SerializeField] string testItemTitle;
    [SerializeField] string testItemDescription;

    public void InitInventoryItemsUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            var itemUI = Instantiate(itemPrefab, contentPanel);
            itemUI.transform.ResetTransform();
            inventoryUIList.Add(itemUI);
            itemUI.SetUpActions(HandleClickedItem, HandleRightClickedItem, HandleDropItem, HandleBeginDragItem, HandleEndDragItem);
        }
    }

    private void HandleEndDragItem(UIItem item)
    {
    }

    private void HandleBeginDragItem(UIItem item)
    {
    }

    private void HandleDropItem(UIItem item)
    {
    }

    private void HandleClickedItem(UIItem item)
    {
        Debug.Log("Clicked");
        uiInventoryDescription.SetDescription(testItemSprite, testItemTitle, testItemDescription);
    }

    private void HandleRightClickedItem(UIItem item)
    {
    }

    public void Show()
    {
        uiInventoryDescription.ResetDescription();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}

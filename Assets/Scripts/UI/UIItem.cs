using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Image selectedImage;
    [SerializeField] TMP_Text quantityText;
    private bool isSelected;
    private bool isEmpty;
    public event Action<UIItem> OnItemClicked, OnRightMouseItemClicked, OnItemDropOn, OnItemBeginDrag, OnItemEndDrag;

    public void SetUpActions(Action<UIItem> OnItemClicked, Action<UIItem> OnRightMouseItemClicked, Action<UIItem> OnItemDropOn, Action<UIItem> OnItemBeginDrag, Action<UIItem> OnItemEndDrag)
    {
        this.OnItemClicked = OnItemClicked;
        this.OnRightMouseItemClicked = OnRightMouseItemClicked;
        this.OnItemDropOn = OnItemDropOn;
        this.OnItemBeginDrag = OnItemBeginDrag;
        this.OnItemEndDrag = OnItemEndDrag;
    }

    public void SetData(Sprite itemSprite, int itemQuantity)
    {
        itemImage.sprite = itemSprite;
        quantityText.text = itemQuantity.ToString();
        isEmpty = false;
    }

    public void ResetData()
    {
        itemImage.sprite = null;
        quantityText.text = string.Empty;
    }

    public void Select()
    {
        selectedImage.gameObject.SetActive(true);
    }

    public void UnSelect()
    {
        selectedImage.gameObject.SetActive(false);
    }

    public void OnBeginDrag()
    {
        if (isEmpty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        OnItemDropOn?.Invoke(this);
    }

    public void OnPointerClicked(BaseEventData eventData)
    {
        PointerEventData pointerEventData = (PointerEventData)eventData;
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseItemClicked?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIItem : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Image selectedImage;
        [SerializeField] TMP_Text quantityText;
        private bool isEmpty;
        public event Action<UIItem> OnItemClicked, OnRightMouseItemClicked, OnItemDropOn, OnItemBeginDrag, OnItemEndDrag;
        private Sprite itemSprite;
        private int itemQuantity;

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
            isEmpty = false;
            itemImage.sprite = itemSprite;
            quantityText.text = itemQuantity.ToString();
            this.itemSprite = itemSprite;
            this.itemQuantity = itemQuantity;
            itemImage.gameObject.SetActive(true);
            quantityText.gameObject.SetActive(itemQuantity > 0);

        }

        public void ResetData()
        {
            itemImage.sprite = null;
            quantityText.text = string.Empty;
            itemImage.gameObject.SetActive(false);
        }

        public void Select()
        {
            selectedImage.gameObject.SetActive(true);
        }

        public void UnSelect()
        {
            selectedImage.gameObject.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDropOn?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerEventData pointerEventData = eventData;
            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseItemClicked?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}
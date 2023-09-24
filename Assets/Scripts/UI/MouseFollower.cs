using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] UIItem uIItem;
        private Canvas canvas;

        void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
        }

        public void SetData(Sprite itemSprite, int itemQuantity)
        {
            uIItem.SetData(itemSprite, itemQuantity);
        }

        void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera, out position);
            transform.position = canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }
    }
}
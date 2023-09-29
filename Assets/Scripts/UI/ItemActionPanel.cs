using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        [SerializeField] Button buttonPrefab;

        public void AddActionButton(string actionName, Action onClickAtction)
        {
            var newButton = Instantiate(buttonPrefab, transform);
            newButton.onClick.AddListener(() => onClickAtction());
            var actionNameText = newButton.GetComponentInChildren<TMP_Text>();
            if (actionNameText != null)
            {
                actionNameText.text = actionName;
            }
        }

        public void ToggleButton(bool isActive)
        {
            if(isActive)
                DeleteOldButtons();
            gameObject.SetActive(isActive);
        }

        private void DeleteOldButtons()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
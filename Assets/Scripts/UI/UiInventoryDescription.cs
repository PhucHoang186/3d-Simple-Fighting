using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventoryDescription : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;

    public void ResetDescription()
    {
        itemImage.gameObject.SetActive(false);
        itemImage.sprite = null;
        titleText.text = string.Empty;
        descriptionText.text = string.Empty;
    }

    public void SetDescription(Sprite itemSprite, string itemTitle, string itemDescription)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = itemSprite;
        titleText.text = itemTitle;
        descriptionText.text = itemDescription;
    }
}

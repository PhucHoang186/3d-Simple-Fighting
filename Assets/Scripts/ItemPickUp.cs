using System.Collections;
using System.Collections.Generic;
using Inventory.Data;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [field : SerializeField]
    public ItemData ItemData { get; set; }
    [field : SerializeField]
    public int ItemQuantity { get; set; }
    [SerializeField] private Transform modelContainer;

    void Start()
    {
        var itemObj = Instantiate(ItemData.itemPrefab, modelContainer);
        itemObj.transform.ResetTransform();
    }

    public void OnPickUp()
    {
        PlayPickUpAnimation();
    }

    private void PlayPickUpAnimation()
    {
        modelContainer.gameObject.SetActive(false);
    }
}

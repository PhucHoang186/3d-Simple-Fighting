using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    [SerializeField] UIInventoryTab inventoryTab;
    [SerializeField] int inventorySize;

    private void Start()
    {
        inventoryTab.InitInventoryItemsUI(inventorySize);
        inventoryTab.Show();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(!inventoryTab.isActiveAndEnabled)
            {
                inventoryTab.Show();
            }
            else
            {
                inventoryTab.Hide();
            }
        }
    }
}

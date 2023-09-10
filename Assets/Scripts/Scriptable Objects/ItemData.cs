using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Item itemPrefab;
    public Sprite itemIcon;

    public Item SpawnItem()
    {
        var item =  Instantiate(itemPrefab);
        return item;
    }
}

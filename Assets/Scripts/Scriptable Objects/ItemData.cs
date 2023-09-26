using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [field: SerializeField]
    public bool IsStackable { get; set; }
    public int ID => GetInstanceID();
    [field: SerializeField]
    public int MaxStackSize { get; set; }

    [field: SerializeField]
    public string ItemName { get; set; }
    [field: SerializeField]
    [field: TextArea]
    public string ItemDescription{ get; set; }
    [field: SerializeField]
    public Sprite ItemIcon { get; set; }
    public GameObject itemPrefab;

    public GameObject SpawnItem()
    {
        var item = Instantiate(itemPrefab);
        return item;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data")]
public class ItemData : ScriptableObject
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
    public Item itemPrefab;

    public Item SpawnItem()
    {
        var item = Instantiate(itemPrefab);
        return item;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum EquipmentType
{
    Armor,
    Helmet,
    Weapon,
    Shield,
}

public class Equipment : MonoBehaviour
{
    public EquipmentType equipmentType;

    public void Equip(Transform parent, Equipment oldEquipment)
    {
        if (oldEquipment != null)
        {
            Destroy(oldEquipment.gameObject);
        }
        transform.parent = parent;
        transform.ResetTransform();
    }

    public void UnEquip()
    {
        Destroy(this.gameObject);
    }
}

using System.Collections;
using Entity;
using System.Collections.Generic;
using UnityEngine;
using System;
using Inventory.Data;

[RequireComponent(typeof(EntityCustomize))]
public class EntityHandleEquipment : MonoBehaviour
{
    [SerializeField] protected EntityCustomize entityCustomize;
    [SerializeField] protected List<EquippableItemData> defaultEquipments;

    protected List<GameObject> allEquipments = new();
    public Armor Armor { get; protected set; }
    public Armor Helmet { get; protected set; }
    public Weapon Weapon { get; protected set; }
    public Shield Shield { get; protected set; }

    protected void Start()
    {
        foreach (var equipmentData in defaultEquipments)
        {
            var item = Equip(equipmentData);
            allEquipments.Add(item.gameObject);
        }
    }

    public virtual Equipment Equip(EquippableItemData equipmentData, List<ItemParameter> itemState = null)
    {
        var equipmentObj = equipmentData.SpawnItem();
        if (equipmentObj.TryGetComponent<Equipment>(out var equipment))
        {
            switch ((equipment.equipmentType))
            {
                case EquipmentType.Armor:
                    PutOnEquipment(equipment, Armor);
                    Armor = ((Armor)equipment);
                    break;
                case EquipmentType.Helmet:
                    PutOnEquipment(equipment, Helmet);
                    Helmet = ((Armor)equipment);
                    break;
                case EquipmentType.Weapon:
                    PutOnEquipment(equipment, Weapon);
                    Weapon = ((Weapon)equipment);

                    break;
                case EquipmentType.Shield:
                    PutOnEquipment(equipment, Shield);
                    Shield = ((Shield)equipment);
                    break;
                default:
                    break;
            }
            return equipment;
        }
        return null;
    }

    protected void PutOnEquipment(Equipment newEquipment, Equipment currentEquipment)
    {
        entityCustomize.WearEquipment(newEquipment, currentEquipment);
    }
}

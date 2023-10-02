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
    public Armor Armor { get; private set; }
    public Armor Helmet { get; private set; }
    public Weapon Weapon { get; private set; }
    public Shield Shield { get; private set; }

    private void Start()
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
                    EquipArmor((Armor)equipment);
                    break;
                case EquipmentType.Helmet:
                    EquipHelmet((Armor)equipment);
                    break;
                case EquipmentType.Weapon:
                    EquipWeapon((Weapon)equipment);
                    break;
                case EquipmentType.Shield:
                    EquipShield((Shield)equipment);
                    break;
                default:
                    break;
            }
            return equipment;
        }
        return null;
    }


    private void EquipArmor(Armor armor)
    {
        entityCustomize.WearEquipment(armor, Armor);
        Armor = armor;
    }

    private void EquipHelmet(Armor helmet)
    {
        entityCustomize.WearEquipment(helmet, Helmet);
        Helmet = helmet;
    }

    private void EquipWeapon(Weapon weapon)
    {
        entityCustomize.WearEquipment(weapon, Weapon);
        Weapon = weapon;
        // weapon.OnHitTarget = onHitTargetCb;
    }

    private void EquipShield(Shield shield)
    {
        entityCustomize.WearEquipment(shield, Shield);
        Shield = shield;
    }
}

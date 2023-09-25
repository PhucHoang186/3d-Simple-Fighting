using System.Collections;
using Entity;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityHandleEquipment : MonoBehaviour
{
    [SerializeField] EntityCustomize entityCustomize;
    [SerializeField] List<GameObject> allEquipments = new();
    [SerializeField] List<ItemData> starterEquipments;
    public Armor Armor { get; private set; }
    public Armor Helmet { get; private set; }
    public Weapon Weapon { get; private set; }
    public Shield Shield { get; private set; }

    private void Start()
    {
        foreach (var equipmentData in starterEquipments)
        {
            var item = Equip((equipmentData));
            allEquipments.Add(item.gameObject);

            if (item.equipmentType == EquipmentType.Weapon)
                Weapon = (Weapon)item;
            else if (item.equipmentType == EquipmentType.Shield)
                Shield = (Shield)item;
        }
    }

    public Equipment Equip(ItemData equipmentData)
    {
        var equipment = (Equipment)equipmentData.SpawnItem();
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

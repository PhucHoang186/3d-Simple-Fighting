using System.Collections;
using System.Collections.Generic;
using Entity;
using NaughtyAttributes;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    [SerializeField] EntityCustomize entityCustomize;
    [SerializeField] List<Equipment> equipmentPrefs;
    [SerializeField] List<GameObject> allEquipments = new List<GameObject>();
    [SerializeField] List<ItemData> starterEquipments;

    private void Start()
    {
        foreach (var equipment in starterEquipments)
        {
            var item =  (Equipment)equipment.SpawnItem();
            entityCustomize.SetEquipment(item);
            allEquipments.Add(item.gameObject);
        }
    }

    // [Button]
    // public void EquipWeapon()
    // {
    //     var weapon = Instantiate(equipmentPrefs[0]);
    //     entityCustomize.SetEquipment(weapon);
    //     allEquipments.Add(weapon.gameObject);
    // }

    // [Button]
    // public void EquipHelmet()
    // {
    //     var helmet = Instantiate(equipmentPrefs[1]);
    //     entityCustomize.SetEquipment(helmet);
    //     allEquipments.Add(helmet.gameObject);
    // }

    // [Button]
    // public void EquipArmor()
    // {
    //     var armor = Instantiate(equipmentPrefs[2]);
    //     entityCustomize.SetEquipment(armor);
    //     allEquipments.Add(armor.gameObject);
    // }

    // [Button]
    // public void EquipShield()
    // {
    //     var shield = Instantiate(equipmentPrefs[3]);
    //     entityCustomize.SetEquipment(shield);
    //     allEquipments.Add(shield.gameObject);
    // }

    // [Button]
    // public void DeleteAllEquipments()
    // {
    //     entityCustomize.ResetEquipment();
    //     foreach (var item in allEquipments)
    //     {
    //         Destroy(item);
    //     }
    //     allEquipments.Clear();
    // }
}

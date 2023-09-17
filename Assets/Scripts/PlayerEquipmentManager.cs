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
}

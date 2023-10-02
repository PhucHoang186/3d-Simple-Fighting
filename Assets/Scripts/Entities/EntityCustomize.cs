using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Entity
{
    public class EntityCustomize : MonoBehaviour
    {
        [SerializeField] List<GameObject> defaultBodyParts;
        [SerializeField] Transform helmetPoint;
        [SerializeField] Transform armorPoint;
        [SerializeField] Transform leftArmPoint;
        [SerializeField] Transform rightArmPoint;
        [SerializeField] Transform weaponPoint;
        [SerializeField] Transform shieldPoint;
        private List<Equipment> allEquipments = new();


        public void WearEquipment(Equipment equipment, Equipment oldEquipment = null)
        {
            switch (equipment.equipmentType)
            {
                case EquipmentType.Helmet:
                    equipment.Equip(helmetPoint, oldEquipment);
                    break;
                case EquipmentType.Armor:
                    ((Armor)equipment).EquipMultipleParts(armorPoint, oldEquipment, leftArmPoint, rightArmPoint);
                    DisableDefaultSkin();
                    break;
                case EquipmentType.Weapon:
                    equipment.Equip(weaponPoint, oldEquipment);
                    break;
                case EquipmentType.Shield:
                    equipment.Equip(shieldPoint, oldEquipment);
                    break;
                default:
                    break;
            }
            allEquipments.Add(equipment);
        }

        public void UnWearEquipment(Equipment equipment)
        {
            if (allEquipments.Contains(equipment))
            {
                equipment.UnEquip();
            }
        }

        public void ResetAllEquipment()
        {
            ToggleBodyParts(true);
            
        }

        public void DisableDefaultSkin()
        {
            ToggleBodyParts(false);
        }

        private void ToggleBodyParts(bool isActive)
        {
            foreach (var part in defaultBodyParts)
            {
                part.SetActive(isActive);
            }
        }
    }
}
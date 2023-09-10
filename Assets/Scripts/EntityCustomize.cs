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
        [SerializeField] Transform weaponPoint;
        [SerializeField] Transform shield;
        private Equipment currentHelmet;
        private Equipment currentArmor;
        private Equipment currentWeapon;
        private Equipment currentShield;
        public Weapon Currentweapon => (Weapon)currentWeapon;

        public void SetEquipment(Equipment equipment)
        {
            switch (equipment.equipmentType)
            {
                case EquipmentType.Helmet:
                    equipment.Equip(helmetPoint, currentHelmet);
                    currentHelmet = equipment;
                    break;
                case EquipmentType.Armor:
                    equipment.Equip(armorPoint, currentArmor);
                    currentArmor = equipment;
                    DisableDefaultSkin();
                    break;
                case EquipmentType.Weapon:
                    equipment.Equip(weaponPoint, currentWeapon);
                    currentWeapon = equipment;
                    EntityEvents.OnSetWeapon?.Invoke((Weapon)equipment);
                    break;
                case EquipmentType.Shield:
                    equipment.Equip(shield, currentShield);
                    currentShield = equipment;
                    break;
                default:
                    break;
            }
        }

        public void ResetEquipment()
        {
            ToggleBodyParts(true);
            DestroyEquipment(currentArmor);
            DestroyEquipment(currentHelmet);
            DestroyEquipment(currentWeapon);
            DestroyEquipment(currentShield);
        }

        private void DestroyEquipment(Equipment equipment)
        {
            if(equipment != null)
                Destroy(equipment.gameObject);
            equipment = null;
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
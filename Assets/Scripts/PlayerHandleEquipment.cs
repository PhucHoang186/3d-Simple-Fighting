using System.Collections;
using System.Collections.Generic;
using Inventory.Data;
using UnityEngine;
namespace Entity
{
    public class PlayerHandleEquipment : EntityHandleEquipment
    {
        [SerializeField] protected EntityCustomize entityUICustomize;
        [SerializeField] List<ItemParameter> parametersToModify;
        [SerializeField] InventoryData inventoryData;
        private List<ItemParameter> currentItemState;

        public override Equipment Equip(EquippableItemData equipmentData, List<ItemParameter> itemState = null)
        {
            if (itemState != null)
                currentItemState = new List<ItemParameter>(itemState);
            var equipmentObj = equipmentData.SpawnItem();
            var equipmentObjUI = equipmentData.SpawnItem();

            // ModifyParameters();
            inventoryData.AddItem(equipmentData, 1, currentItemState);

            if (equipmentObj.TryGetComponent<Equipment>(out var equipment))
            {
                if (equipmentObjUI.TryGetComponent<Equipment>(out var equipmentUI))
                {
                    switch ((equipment.equipmentType))
                    {
                        case EquipmentType.Armor:
                            PutOnEquipment(equipment, Armor);
                            PutOnEquipmentForUI(equipmentUI, Armor);
                            Armor = ((Armor)equipment);
                            break;
                        case EquipmentType.Helmet:
                            PutOnEquipment(equipment, Helmet);
                            PutOnEquipmentForUI(equipmentUI, Helmet);
                            Helmet = ((Armor)equipment);
                            break;
                        case EquipmentType.Weapon:
                            PutOnEquipment(equipment, Weapon);
                            PutOnEquipmentForUI(equipmentUI, Weapon);
                            Weapon = ((Weapon)equipment);

                            break;
                        case EquipmentType.Shield:
                            PutOnEquipment(equipment, Shield);
                            PutOnEquipmentForUI(equipmentUI, Shield);
                            Shield = ((Shield)equipment);
                            break;
                        default:
                            break;
                    }
                    return equipment;
                }
            }
            return null;
        }

        protected void PutOnEquipmentForUI(Equipment newEquipment, Equipment currentEquipment)
        {
            entityUICustomize.WearEquipment(newEquipment, currentEquipment);
        }

        public void ModifyParameters()
        {
            for (int i = 0; i < currentItemState.Count; i++)
            {
                if (parametersToModify.Contains(currentItemState[i]))
                {
                    float newValue = currentItemState[i].value + parametersToModify[parametersToModify.IndexOf(currentItemState[i])].value;
                    currentItemState[i] = new ItemParameter
                    {
                        parameterData = parametersToModify[parametersToModify.IndexOf(currentItemState[i])].parameterData,
                        value = newValue
                    };
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Inventory.Data;
using UnityEngine;
namespace Entity
{
    public class PlayerHandleEquipment : EntityHandleEquipment
    {
        [SerializeField] List<ItemParameter> parametersToModify, currentItemState;
        [SerializeField] InventoryData inventoryData;

        public override Equipment Equip(EquippableItemData equipmentData, List<ItemParameter> itemState = null)
        {
            if (itemState != null)
                currentItemState = new List<ItemParameter>(itemState);
            ModifyParameters();
            inventoryData.AddItem(equipmentData, 1, currentItemState);
            return base.Equip(equipmentData, itemState);
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

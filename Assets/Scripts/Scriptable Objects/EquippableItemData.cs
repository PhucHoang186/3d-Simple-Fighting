using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Inventory.Data
{
    [CreateAssetMenu]
    public class EquippableItemData : ItemData, IDestroyableItem, IItemAction
    {
        public string ActionName => "Equip";

        public AudioClip ActionSfx { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            if(character.TryGetComponent<PlayerHandleEquipment>(out PlayerHandleEquipment handleEquipment))
            {
                handleEquipment.Equip(this, itemState);
                return true;
            }
            return false;
        }
    }
}

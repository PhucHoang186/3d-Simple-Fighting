using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Data
{
    [CreateAssetMenu(menuName = "Consumalbe Item")]
    public class ConsumableItemData : ItemData, IDestroyableItem, IItemAction
    {
        [SerializeField] List<ModifierData> modifierDatas;
        public string ActionName => "Consume";
        public AudioClip ActionSfx { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach (var modifierData in modifierDatas)
            {
                modifierData.statModifier.AffterPlayer(character, modifierData.value);
            }
            return true;
        }
    }

    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip ActionSfx { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }


    [Serializable]
    public class ModifierData
    {
        public PlayerStatModifier statModifier;
        public float value;
    }
}

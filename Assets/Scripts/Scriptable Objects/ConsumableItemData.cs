using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Data
{
    [CreateAssetMenu(menuName = "Consumalbe Item")]
    public class ConsumableItemData : ItemData, IDestroyableItem, IItemAtction
    {
        [SerializeField] List<ModifierData> modifierDatas;
        public string ActionName => "Consume";
        public AudioClip ActionSfx { get; private set; }

        public bool PerformAction(GameObject character)
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

    public interface IItemAtction
    {
        public string ActionName { get; }
        public AudioClip ActionSfx { get; }
        bool PerformAction(GameObject character);
    }


    [Serializable]
    public class ModifierData
    {
        public PlayerStatModifier statModifier;
        public float value;
    }
}

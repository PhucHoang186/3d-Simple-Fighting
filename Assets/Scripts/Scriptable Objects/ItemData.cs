using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Data
{
    public abstract class ItemData : ScriptableObject
    {
        [field: SerializeField]
        public bool IsStackable { get; set; }
        public int ID => GetInstanceID();
        [field: SerializeField]
        public int MaxStackSize { get; set; }

        [field: SerializeField]
        public string ItemName { get; set; }
        [field: SerializeField]
        [field: TextArea]
        public string ItemDescription { get; set; }
        [field: SerializeField]
        public List<ItemParameter> DefaultParameterList { get; set; }
        [field: SerializeField]
        public Sprite ItemIcon { get; set; }
        [field: SerializeField]
        public GameObject ItemPrefab { get; set; }

        public GameObject SpawnItem()
        {
            var item = Instantiate(ItemPrefab);
            return item;
        }
    }

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterData parameterData;
        public float value;

        public bool Equals(ItemParameter otherItemParameter)
        {
            return otherItemParameter.parameterData == parameterData;
        }
    }
}
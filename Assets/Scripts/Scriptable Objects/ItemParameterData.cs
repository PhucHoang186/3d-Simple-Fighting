using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Data
{
[CreateAssetMenu]
    public class ItemParameterData : ScriptableObject
    {
        [field : SerializeField]
        public string ParameterName {get; set;}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(menuName = "Entity Stat Data")]
    public class EntityStatData : ScriptableObject
    {
        public float maxHealth;
        public float movementSpeed;
        public float slowSpeed;
        public float sprintSpeed;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(menuName = "Entity Data")]
    public class EntityData : ScriptableObject
    {
        public float maxHealth;
        public float movementSpeed;
        public float rotateSpeed;

    }
}
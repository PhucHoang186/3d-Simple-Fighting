using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(EnemyAI))]
    public class EnemyHandleInput : EntityHandleInput
    {
        [SerializeField] EnemyAI enemyAI;
        public override EntityInput GetInput()
        {
            return enemyAI.GetEnemyAIInput();
        }
    }
}
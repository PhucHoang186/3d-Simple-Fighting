using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class EnemyHandleInput : EntityHandleInput
    {
        [SerializeField] EnemyAI enemyAI;
        public override EntityInput GetInput()
        {
            return enemyAI.GetEnemyAIInput();
        }
    }
}
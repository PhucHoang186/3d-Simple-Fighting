using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Entity
{
    public class EnemyHandleTakenDamage : EntityHandleTakenDamage
    {
        [SerializeField] EnemyWorldSpaceHealthUI enemyHealthUI;

        public override void InitHealth(float maxHealth)
        {
            base.InitHealth(maxHealth);
            enemyHealthUI.Init(maxHealth);
        }

        public override void TakenDamage(float damageAmount = 1, Vector3 hitPoint = default(Vector3))
        {
            base.TakenDamage(damageAmount, hitPoint);
            enemyHealthUI.Health = currentHealth;
        }

    }
}
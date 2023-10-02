using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Entity
{
    public class UniqueEnemyHandleAttack : EntityHandleAttack
    {
        [SerializeField] bool blockable;
        [SerializeField] bool meleeType;
        [SerializeField] bool rangeType;
        [ShowIf("meleeType")] Collider weaponCollider; 
        [ShowIf("rangeType")] SpellData initSpell;
        [ShowIf("rangeType")] Transform castSpellPoint;

        public override void HandleAttackInput(Entity entity, EntityInput entityInput)
        {
            Attack(entity, entityInput);
            if (blockable)
                Block(entity, entityInput);
        }

        protected override void Block(Entity entity, EntityInput entityInput)
        {
            if (entityInput.isBlockPressed)
            {
                if (isBlocking)
                    return;
                isBlocking = true;
                entity.ChangeEntityState(EntityState.Entity_Block);
            }
            else
            {
                if (!isBlocking)
                    return;
                isBlocking = false;
                // handleEquipment.Shield.ToggleShieldHitBox(false);
                entity.ChangeEntityState(EntityState.Entity_UnBlock);
            }
        }

        protected override void Attack(Entity entity, EntityInput entityInput)
        {
            if (!entityInput.StartAttack)
                return;

            // melee
            if (meleeType)
            {
                if (entityInput.isInstantAttackPressed)
                {
                    entity.ChangeEntityState(EntityState.Entity_Attack_Short, 1f);
                    return;
                }
            }
            // range
            else if (rangeType)
            {
                if (entityInput.isCastingAttackPressed)
                {
                    ChargingAttack(entity);
                }
            }
        }

        protected override void ChargingAttack(Entity entity)
        {
            if (startCharging)
                return;
            startCharging = true;
            entity.ChangeEntityState(EntityState.Entity_Attack_Long);
        }


        protected override void Activate(Entity entity)
        {

            entity.ChangeEntityState(EntityState.Entity_UnAttack_Long);
        }

        protected void ActivateSpell()
        {
            var spellUsed = Instantiate(initSpell.spellPrefab, castSpellPoint);
            spellUsed.transform.ResetTransform();
            spellUsed.Init(0f, initSpell.spellSpeed, OnHitTarget);
        }

        protected void OnHitTarget(IDamageable damageable, Vector3 hitPoint = default)
        {
            // damageable.TakenDamage(WeaponTrueDamage(), hitPoint);
        }
    }
}
using UnityEngine;

namespace Entity
{
    public class EntityHandleAttack : MonoBehaviour
    {
        [SerializeField] protected EntityHandleEquipment handleEquipment;
        [SerializeField] protected EntityHandleSpellSystem spellSystem;
        protected bool isBlocking;
        protected float chargingTime;
        protected bool startCharging;
        protected bool finishCharging;

        // for melee weapon
        // use in animation event
        public void ToggleHitBoxOn()
        {
            if (!handleEquipment.Weapon.IsChargingTypeWeapon())
            {
                ((MeleeWeapon)handleEquipment.Weapon).ToggleHitBox(true);
            }
        }

        public void ToggleHitBoxOff()
        {
            if (!handleEquipment.Weapon.IsChargingTypeWeapon())
            {
                ((MeleeWeapon)handleEquipment.Weapon).ToggleHitBox(false);
            }
        }

        public virtual void HandleAttackInput(Entity entity, EntityInput entityInput)
        {
            if (handleEquipment.Weapon != null)
                Attack(entity, entityInput);
            if (handleEquipment.Shield != null)
                Block(entity, entityInput);
        }

        protected virtual void Block(Entity entity, EntityInput entityInput)
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
                handleEquipment.Shield.ToggleShieldHitBox(false);
                entity.ChangeEntityState(EntityState.Entity_UnBlock);
            }
        }

        protected virtual void Attack(Entity entity, EntityInput entityInput)
        {
            if (!entityInput.StartAttack)
                return;

            var isChargingWeaponType = handleEquipment.Weapon.IsChargingTypeWeapon();
            // melee
            if (entityInput.isInstantAttackPressed && !isChargingWeaponType)
            {
                entity.ChangeEntityState(EntityState.Entity_Attack_Short, 1f);
                return;
            }

            // range
            if (isChargingWeaponType)
            {
                if (entityInput.isCastingAttackPressed)
                {
                    ChargingAttack(entity);
                }

                if (entityInput.isCastingAttackReleased)
                {
                    if (finishCharging)
                    {
                        Activate(entity);
                    }
                    else
                    {
                        DeActivate(entity);
                    }
                    startCharging = false;
                    finishCharging = false;
                }
            }
        }

        protected virtual void Activate(Entity entity)
        {
            entity.ChangeEntityState(EntityState.Entity_UnAttack_Long);
            ((RangeWeapon)handleEquipment.Weapon).ActivateSkill();
        }

        protected virtual void DeActivate(Entity entity)
        {
            entity.ChangeEntityState(EntityState.Entity_UnAttack_Long);
            ((RangeWeapon)handleEquipment.Weapon).DeActivateSkill();
        }

        protected virtual void ChargingAttack(Entity entity)
        {
            if (startCharging)
                return;
            startCharging = true;
            chargingTime = spellSystem.GetCurrentSpellData().castTime;
            entity.ChangeEntityState(EntityState.Entity_Attack_Long);
        }

        protected void Update()
        {
            // callBackStateAfterAttack = isBlocking ? EntityState.Entity_Default : EntityState.Entity_Idle;
            if (!startCharging || finishCharging)
                return;
            if (chargingTime <= 0)
                OnFinishCharging();
            else
                chargingTime -= Time.deltaTime;
        }

        protected void OnFinishCharging()
        {
            finishCharging = true;
            ((RangeWeapon)handleEquipment.Weapon).OnFinishCharge();
        }

        // use animation event , play at the end of casting animation
        public void StartCastingSpell()
        {
            var rangeWeapon = ((RangeWeapon)handleEquipment.Weapon);
            rangeWeapon.SetSpellData(spellSystem.GetCurrentSpellData());
            rangeWeapon.Charging();
        }

        public void StartBlocking()
        {
            handleEquipment.Shield.ToggleShieldHitBox(true);
        }

        public void StopBlocking()
        {
            handleEquipment.Shield.ToggleShieldHitBox(false);
        }

    }
}
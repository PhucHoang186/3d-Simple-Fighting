using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entity;
using NaughtyAttributes;
using UnityEngine;

public class EntityHandleAttack : MonoBehaviour
{
    [SerializeField] protected Weapon defaultWeapon;
    [SerializeField] protected EntityHandleSpellSystem spellSystem;
    protected Weapon currentWeapon;
    protected bool isBlocking;
    protected float chargingTime;
    protected bool startCharging;
    protected bool finishCharging;

    void Start()
    {
        EntityEvents.OnSetWeapon += OnSetWeapon;
        EntityEvents.OnSetWeapon?.Invoke(defaultWeapon);
    }

    void OnDestroy()
    {
        EntityEvents.OnSetWeapon -= OnSetWeapon;
        if (currentWeapon != null)
            currentWeapon.OnHitTarget = null;
    }


    public void OnSetWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        if (currentWeapon != null)
            currentWeapon.OnHitTarget = OnHitTarget;
    }

    // for melee weapon
    // use in animation event
    public void ToggleHitBoxOn()
    {
        if (!currentWeapon.IsChargingTypeWeapon())
        {
            ((MeleeWeapon)currentWeapon)?.ToggleHitBox(true);
        }
    }

    public void ToggleHitBoxOff()
    {
        if (!currentWeapon.IsChargingTypeWeapon())
        {
            ((MeleeWeapon)currentWeapon)?.ToggleHitBox(false);
        }
    }

    public void HandleAttackInput(Entity.Entity entity, EntityInput entityInput)
    {
        if (currentWeapon == null)
            return;
        Attack(entity, entityInput);
        Block(entity, entityInput);
    }

    protected void Block(Entity.Entity entity, EntityInput entityInput)
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
            entity.ChangeEntityState(EntityState.Entity_UnBlock);
        }
    }

    protected void Attack(Entity.Entity entity, EntityInput entityInput)
    {
        if (!entityInput.StartAttack)
            return;

        var isChargingWeaponType = currentWeapon.IsChargingTypeWeapon();
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
                startCharging = false;
                finishCharging = false;
            }
        }
    }

    protected void Activate(Entity.Entity entity)
    {
        if (finishCharging)
        {
            entity.ChangeEntityState(EntityState.Entity_UnAttack_Long);
            ((RangeWeapon)currentWeapon).ActivateSkill();
        }
        else
        {
            entity.ChangeEntityState(EntityState.Entity_UnAttack_Long);
            ((RangeWeapon)currentWeapon).DeActivateSkill();
        }
    }

    protected void ChargingAttack(Entity.Entity entity)
    {
        if (startCharging)
            return;
        startCharging = true;
        chargingTime = spellSystem.GetCurrentSpellData().chargingTime;
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
        ((RangeWeapon)currentWeapon).OnFinishCharge();
    }

    public void OnHitTarget(Collider targetCol, Vector3 hitPoint = default)
    {
        if (targetCol.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakenDamage(currentWeapon.WeaponTrueDamage(), hitPoint);
        }
    }

    // use animation event , play at the end of casting animation
    public void StartCastingSpell()
    {
        var rangeWeapon = ((RangeWeapon)currentWeapon);
        rangeWeapon.SetSpellData(spellSystem.GetCurrentSpellData());
        rangeWeapon.Charging();
    }
}

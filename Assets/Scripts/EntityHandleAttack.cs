using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entity;
using NaughtyAttributes;
using UnityEngine;

public class EntityHandleAttack : MonoBehaviour
{
    private Weapon currentWeapon;
    private bool isHoldingAttack;
    private bool isBlocking;

    void Start()
    {
        EntityEvents.OnSetWeapon += OnSetWeapon;
    }

    void OnDestroy()
    {
        EntityEvents.OnSetWeapon -= OnSetWeapon;

        if (currentWeapon != null)
            currentWeapon.OnHitTarget += OnHitTarget;
    }


    public void OnSetWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        if (currentWeapon != null)
            currentWeapon.OnHitTarget += OnHitTarget;
    }

    // for melee weapon
    // use in animation event
    public void ToggleHitBoxOn()
    {
        if (!currentWeapon.IsCastingTypeWeapon())
        {
            ((MeleeWeapon)currentWeapon)?.ToggleHitBox(true);
        }
    }

    public void ToggleHitBoxOff()
    {
        if (!currentWeapon.IsCastingTypeWeapon())
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

    private void Block(Entity.Entity entity, EntityInput entityInput)
    {
        if (entityInput.isBlockPressed)
        {
            if (isBlocking)
                return;
            isBlocking = true;
            entity.ChangeEntityState(EntityState.Entity_Defend);
        }
        else
        {
            if (!isBlocking)
                return;
            isBlocking = false;
            entity.ChangeEntityState(EntityState.Entity_Idle);
        }
    }

    private void Attack(Entity.Entity entity, EntityInput entityInput)
    {
        if (!entityInput.StartAttack)
            return;

        // melee
        var isCastingWeaponType = currentWeapon.IsCastingTypeWeapon();
        if (entityInput.isInstantAttackPressed && !isCastingWeaponType)
        {
            entity.ChangeEntityState(EntityState.Entity_Attack_Short, 1f);
            return;
        }

        // range
        if (entityInput.isCastingAttackPressed)
        {
            if (isCastingWeaponType)
            {
                isHoldingAttack = true;
                entity.ChangeEntityState(EntityState.Entity_Attack_Long);
            }
        }
        if (entityInput.isCastingAttackReleased)
        {
            if (isHoldingAttack)
            {
                isHoldingAttack = false;
                Debug.Log("Shoot Spell");
                entity.ChangeEntityState(EntityState.Entity_Idle);
            }
        }
    }

    public void OnHitTarget(Collider targetCol, Vector3 hitPoint = default(Vector3))
    {
        var damageable = targetCol.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakenDamage(currentWeapon.WeaponTrueDamage(), hitPoint);
        }
    }
}

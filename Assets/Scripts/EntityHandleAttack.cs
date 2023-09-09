using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class EntityHandleAttack : MonoBehaviour
{
    [SerializeField] protected float attackDuraction = 1;
    private Weapon currentWeapon;
    private bool isHoldingAttack;

    void Start()
    {
        currentWeapon = GetComponentInChildren<Weapon>();
        if (currentWeapon != null)
            currentWeapon.OnHitTarget += OnHitTarget;
    }

    void OnDestroy()
    {
        if (currentWeapon != null)
            currentWeapon.OnHitTarget += OnHitTarget;
    }

    public void ToggleHitBoxOn()
    {
        currentWeapon?.ToggleHitBox(true);
    }

    public void ToggleHitBoxOff()
    {
        currentWeapon?.ToggleHitBox(false);
    }

    public void HandleAttackInput(EntityCustomize customize, bool instantAttackPressed, bool castingAttackPressed)
    {
        // melee
        var isCastingWeapon = currentWeapon.IsCastingTypeWeapon();
        if (instantAttackPressed && !isCastingWeapon)
        {
            customize.PlayAnim(EntityAnimation.Character_Attack, attackDuraction);
            return;
        }

        // range
        if (castingAttackPressed)
        {
            if (isCastingWeapon)
            {
                isHoldingAttack = true;
                customize.PlayAnim(EntityAnimation.Character_StartCasting);
            }
        }
        else
        {
            if (isHoldingAttack)
            {
                isHoldingAttack = false;
                Debug.Log("Shoot Spell");
                // customize.PlayAnim(EntityAnimation.Character_Idle);
            }
        }
    }

    public void OnHitTarget(Collider targetCol, Vector3 hitPoint = default(Vector3))
    {
        var damageable = targetCol.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakenDamage(currentWeapon.weaponDamage, hitPoint);
        }
    }
}

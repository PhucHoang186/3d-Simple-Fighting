using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponType
{
    Melee,
    Range,
    Spell,
}

public class Weapon : Equipment
{
    [SerializeField] protected float weaponBaseDamage;
    [SerializeField] protected WeaponType weaponType;
    protected float nullifyAmount;

    public virtual float WeaponTotalDamage()
    {
        return 0f;
    }

    public float WeaponTrueDamage()
    {
        return WeaponTotalDamage() - nullifyAmount;
    }

    public virtual void SetNullifyDamage(float nullifyAmount)
    {
        this.nullifyAmount = nullifyAmount;
    }

    protected void OnHitTarget(IDamageable damageable, Vector3 hitPoint = default)
    {
        damageable.TakenDamage(WeaponTrueDamage(), hitPoint);
        this.nullifyAmount = 0f;
    }


    public bool IsChargingTypeWeapon()
    {
        return weaponType == WeaponType.Spell || weaponType == WeaponType.Range;
    }
}

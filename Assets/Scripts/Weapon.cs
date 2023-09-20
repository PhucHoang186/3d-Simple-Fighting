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
    public WeaponType weaponType;
    public float weaponBaseDamage;

    public void OnHitTarget(IDamageable damageable, Vector3 hitPoint = default)
    {
        damageable.TakenDamage(WeaponTrueDamage(), hitPoint);
    }

    public virtual float WeaponTrueDamage()
    {
        return 0f;
    }

    public bool IsChargingTypeWeapon()
    {
        return weaponType == WeaponType.Spell || weaponType == WeaponType.Range;
    }
}

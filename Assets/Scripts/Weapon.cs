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
    public System.Action<Collider, Vector3> OnHitTarget;
    public WeaponType weaponType;
    public float weaponBaseDamage;

    public virtual float WeaponTrueDamage()
    {
        return 0f;
    }

    public bool IsChargingTypeWeapon()
    {
        return weaponType == WeaponType.Spell || weaponType == WeaponType.Range;
    }
}

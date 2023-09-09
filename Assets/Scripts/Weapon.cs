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

public class Weapon : MonoBehaviour
{
    public System.Action<Collider, Vector3> OnHitTarget;
    public WeaponType weaponType;
    public Collider hitBoxCollider;
    public float weaponDamage;

    private void Start()
    {
        ToggleHitBox(false);
    }

    public void ToggleHitBox(bool isActive)
    {
        hitBoxCollider.enabled = isActive;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {

            OnHitTarget?.Invoke(collider, collider.ClosestPoint(transform.position));
        }
    }

    public bool IsCastingTypeWeapon()
    {
        return weaponType == WeaponType.Spell || weaponType == WeaponType.Range;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Collider hitBoxCollider;

    private void Start()
    {
        ToggleHitBox(false);
    }

    public void ToggleHitBox(bool isActive)
    {
        hitBoxCollider.enabled = isActive;
    }

    public override float WeaponTrueDamage()
    {
        return weaponBaseDamage;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {

            OnHitTarget?.Invoke(collider, collider.ClosestPoint(transform.position));
        }
    }
}
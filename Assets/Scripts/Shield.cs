using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class Shield : Equipment
{
    [SerializeField] Collider shieldCollider;
    [SerializeField] float physicDefend;
    [SerializeField] float magicDefend;
    [SerializeField] GameObject sparkle;
    [SerializeField] float blockCooldown;
    private float currentBlockCooldown;
    private bool canBlock;

    public void ToggleShieldHitBox(bool isActive)
    {
        shieldCollider.enabled = isActive;
    }

    private void Update()
    {
        if (currentBlockCooldown > 0)
            currentBlockCooldown -= Time.deltaTime;
        else
        {
            canBlock = true;
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (!canBlock)
            return;

        if (collider.TryGetComponent<Weapon>(out Weapon weapon))
        {
            currentBlockCooldown = blockCooldown;
            canBlock = false;
            DeflectEntityAttack(weapon);
            NullifyWeaponDamage(weapon, physicDefend);
            return;
        }

        if (collider.TryGetComponent<Spell>(out Spell spell))
        {
            // spell
        }
    }

    private void NullifyWeaponDamage(Weapon weapon, float nullifyAmount)
    {
        weapon.SetNullifyDamage(nullifyAmount);
    }

    private void DeflectEntityAttack(Weapon weapon)
    {
        // weapon.
        var enemy = weapon.GetComponentInParent<Entity.Entity>();
        enemy.ChangeEntityState(Entity.EntityState.Entity_Attack_Deflected, 1f);
        var thisEntity = GetComponentInParent<Entity.Entity>();
        thisEntity.ChangeEntityState(Entity.EntityState.Entity_Blocking_GetHit);
        ShowHitSparkle();
    }

    private void ShowHitSparkle()
    {
        StartCoroutine(CorShowHitSparkle());
    }

    private IEnumerator CorShowHitSparkle()
    {
        sparkle.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        sparkle.SetActive(false);
    }
}

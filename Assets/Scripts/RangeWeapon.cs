using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    // use for bow and staff
    [SerializeField] private Transform castingPoint;
    [SerializeField] private GameObject onChargeFinishVfx;
    private Spell spellUsed;
    private SpellData currentSpellData;



    public void SetSpellData(SpellData spellData)
    {
        currentSpellData = spellData;
    }

    public override float WeaponTotalDamage()
    {
        var spellDamage = currentSpellData != null ? currentSpellData.spellDamage : 0f;
        return weaponBaseDamage + spellDamage;
    }

    public void OnFinishCharge()
    {
        onChargeFinishVfx.SetActive(true);
    }

    public void Charging()
    {
        onChargeFinishVfx.SetActive(false);
        spellUsed = Instantiate(currentSpellData.spellPrefab, castingPoint);
        spellUsed.transform.ResetTransform();
        spellUsed.Init(currentSpellData.castTime, currentSpellData.spellSpeed, OnHitTarget);
    }

    public void DeActivateSkill()
    {
        if (spellUsed != null)
            spellUsed.DeActivateSkill();
    }

    public void ActivateSkill()
    {
        if (spellUsed != null)
        {
            onChargeFinishVfx.SetActive(false);
            spellUsed.ActivateSkill();
        }
    }
}

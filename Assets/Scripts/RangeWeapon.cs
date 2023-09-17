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

    public override float WeaponTrueDamage()
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
        spellUsed.Init(currentSpellData.spellSpeed, WeaponTrueDamage());
    }

    public void DeActivateSkill()
    {
        spellUsed.DeActivateSkill();
    }

    public void ActivateSkill()
    {
        onChargeFinishVfx.SetActive(false);
        onChargeFinishVfx.SetActive(false);
        spellUsed.ActivateSkill();
    }
}

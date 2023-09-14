using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    [SerializeField] private Transform castingPoint;
    [SerializeField] private SpellData currentSpellData;
    private Spell spellUsed;

    public override float WeaponTrueDamage()
    {
        var spellDamage = currentSpellData != null ? currentSpellData.spellDamage : 0f;
        return weaponBaseDamage + spellDamage;
    }

    public void Charging()
    {
        spellUsed = Instantiate(currentSpellData.spellPrefab, castingPoint);
        spellUsed.transform.ResetTransform();
        spellUsed.Init(currentSpellData.spellSpeed);
    }

    public void UseSpell()
    {
        spellUsed.CastSpell();
    }
}

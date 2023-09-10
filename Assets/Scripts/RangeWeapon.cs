using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    private Spell currentUseSpell;
    public override float WeaponTrueDamage()
    {
        var spellDamage = currentUseSpell != null ? currentUseSpell.spellDamage : 0f;
        return weaponBaseDamage + spellDamage;
    }
}

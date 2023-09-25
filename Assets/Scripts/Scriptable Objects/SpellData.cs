using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell Data")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public Spell spellPrefab;
    public float spellDamage;
    public float spellSpeed = 2f;
    public float castTime;
}

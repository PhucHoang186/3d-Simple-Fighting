using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandleSpellSystem : MonoBehaviour
{
    [SerializeField] private List<SpellData> spellAvailables = new List<SpellData>();
    private int spellUsedIndex;

    public SpellData GetCurrentSpellData()
    {
        if(spellUsedIndex < spellAvailables.Count)
        {
            return spellAvailables[spellUsedIndex];
        }
        return null;
    }
}

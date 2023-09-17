using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;


public class SpellDatasConfig : MonoBehaviour
{
    [SerializeField] SpellDataDict spellDataDict;

    


}
[Serializable]
public class SpellDataDict : SerializableDictionaryBase<string, SpellData>
{

}

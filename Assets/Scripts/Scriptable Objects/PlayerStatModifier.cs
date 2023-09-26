using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStatModifier : ScriptableObject
{
    public abstract void AffterPlayer(GameObject player, float modifyValue);
}

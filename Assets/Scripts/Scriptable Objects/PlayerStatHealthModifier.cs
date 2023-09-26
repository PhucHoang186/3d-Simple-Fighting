using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatHealthModifier : PlayerStatModifier
{
    public override void AffterPlayer(GameObject player, float modifyValue)
    {
        var handleHealth = player.GetComponent<EntityHandleHealth>();
        if(handleHealth != null)
            handleHealth.Heal((int)modifyValue);
    }
}

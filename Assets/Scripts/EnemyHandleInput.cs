using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

public class EnemyHandleInput : EntityHandleInput
{
    public override EntityInput GetInput()
    {
        var entityInput =  new EntityInput();
        return entityInput;
    }
}

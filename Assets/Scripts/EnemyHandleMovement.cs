using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandleMovement : EntityHandleMovement
{
    public override void Rotate(Vector3 rotateVec)
    {
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, rotateVec, rotateSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}

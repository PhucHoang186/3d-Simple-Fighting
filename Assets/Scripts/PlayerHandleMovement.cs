using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandleMovement : EntityHandleMovement
{
    [SerializeField] protected Transform playerCamera;
    [SerializeField] protected float clampXRotationMin;
    [SerializeField] protected float clampXRotationMax;

    public override void Rotate(Vector3 rotateVec)
    {
        xRotation = Mathf.Lerp(xRotation, xRotation - rotateVec.y * rotateSpeed, Time.deltaTime);
        yRotation = Mathf.Lerp(yRotation, yRotation + rotateVec.x * rotateSpeed, Time.deltaTime);
        xRotation = Mathf.Clamp(xRotation, clampXRotationMin, clampXRotationMax);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0);
    }
}

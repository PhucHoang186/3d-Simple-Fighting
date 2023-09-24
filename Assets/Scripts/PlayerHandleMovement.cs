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
        xRotation = Mathf.Lerp(xRotation, xRotation - rotateVec.y, Time.deltaTime * rotateSpeed);
        yRotation = Mathf.Lerp(transform.eulerAngles.y, transform.eulerAngles.y + rotateVec.x, Time.deltaTime * rotateSpeed);
        xRotation = Mathf.Clamp(xRotation, clampXRotationMin, clampXRotationMax);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
    }
}

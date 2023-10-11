using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandleMovement : EntityHandleMovement
{
    [SerializeField] protected Transform playerCamera;
    [SerializeField] protected float clampXRotationMin;
    [SerializeField] protected float clampXRotationMax;

    public override void Move(Vector3 moveVec)
    {
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, desMoveSpeed, Time.deltaTime * 5f);
        if (Physics.Raycast(transform.position, Vector3.forward * moveVec.z, raycastDistance, collideLayer)) // check back and forth
        {
            moveVec.z = 0f;
            moveVec.x *= slowDownPercent;
        }
        if (Physics.Raycast(transform.position, Vector3.right * moveVec.x, raycastDistance, collideLayer)) // check left and right
        {
            moveVec.x = 0f;
            moveVec.z *= slowDownPercent;
        }
        var newPos = transform.position + moveVec * currentMoveSpeed;
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
    }

    public override void Rotate(Vector3 rotateVec)
    {
        xRotation -= rotateVec.y * rotateSpeed * Time.deltaTime;
        yRotation += rotateVec.x * rotateSpeed * Time.deltaTime;
        // xRotation = Mathf.Lerp(xRotation, xRotation - (rotateVec.y * rotateSpeed), Time.deltaTime * rotateSpeed);
        // yRotation = Mathf.Lerp(yRotation, yRotation + rotateVec.x * rotateSpeed, Time.deltaTime * rotateSpeed);
        xRotation = Mathf.Clamp(xRotation, clampXRotationMin, clampXRotationMax);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0);
    }
}

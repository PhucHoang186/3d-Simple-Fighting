using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class EntityHandleMovement : MonoBehaviour
{
    [SerializeField] Transform model;
    [SerializeField] protected LayerMask collideLayer;
    [SerializeField] protected float raycastDistance = 0.62f;
    [SerializeField] protected float rotateSpeed;
    protected float slowSpeed;
    protected float normalSpeed;
    private Vector3 rotateVec;
    private float currentMoveSpeed;
    private float desMoveSpeed;


    public void Init(EntityStatData entityData)
    {
        normalSpeed = entityData.movementSpeed;
        slowSpeed = entityData.slowSpeed;
    }

    protected void SetMoveSpeed(float moveSpeed)
    {
        desMoveSpeed = moveSpeed;
    }

    public void Move(Vector3 moveVec)
    {
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, desMoveSpeed, Time.deltaTime * 5f);
        if (Physics.Raycast(transform.position, Vector3.forward * moveVec.z, raycastDistance, collideLayer)) // check back and forth
        {
            moveVec.z = 0f;
        }
        if (Physics.Raycast(transform.position, Vector3.right * moveVec.x, raycastDistance, collideLayer)) // check left and right
        {
            moveVec.x = 0f;
        }
        var newPos = transform.position + moveVec.normalized * currentMoveSpeed;
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
    }

    public void Rotate(Vector3 desVec)
    {
        rotateVec = Vector3.Lerp(rotateVec, desVec, rotateSpeed * Time.deltaTime);
        model.rotation = Quaternion.LookRotation(model.forward + rotateVec, Vector3.up);
    }

    public void SetSpeedBaseOnState(EntityState entityState)
    {
        switch (entityState)
        {
            case EntityState.Entity_Idle:
                SetMoveSpeed(normalSpeed);
                break;
            case EntityState.Entity_Attack_Long:
            case EntityState.Entity_Defend:
                SetMoveSpeed(slowSpeed);
                break;
        }
    }
}
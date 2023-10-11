using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class EntityHandleMovement : MonoBehaviour
{
    [SerializeField] protected LayerMask collideLayer;
    [SerializeField] protected float raycastDistance = 0.62f;
    [SerializeField] protected float rotateSpeed;
    [Range(0, 1)]
    [SerializeField] protected float slowDownPercent;

    protected float slowSpeed;
    protected float normalSpeed;
    protected float currentMoveSpeed;
    protected float desMoveSpeed;
    protected float xRotation;
    protected float yRotation;

    public void Init(EntityStatData entityData)
    {
        normalSpeed = entityData.movementSpeed;
        slowSpeed = entityData.slowSpeed;
        desMoveSpeed = entityData.movementSpeed;
    }

    protected void SetMoveSpeed(float desMoveSpeed)
    {
        this.desMoveSpeed = desMoveSpeed;
    }

    public virtual void Move(Vector3 moveVec)
    {
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, desMoveSpeed, Time.deltaTime * 5f);
        var newPos = transform.position + moveVec * currentMoveSpeed;
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
    }

    public virtual void Rotate(Vector3 rotateVec)
    {
        
    }

    public void UpdateMoveSpeed(EntityInput entityInput)
    {
        if (entityInput.isHoldingCombatInput)
            SetMoveSpeed(slowSpeed);
        else
            SetMoveSpeed(normalSpeed);
    }
}

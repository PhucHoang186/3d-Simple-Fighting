using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Net.Security;
using Unity.Mathematics;

namespace Entity
{
    public enum EntityState
    {
        Entity_Default,
        Entity_Idle,
        Entity_Move,
        Entity_Attack_Short,
        Entity_Attack_Long,
        Entity_Block,
        Entity_UnBlock,
        Entity_GetHit,
        Entity_Destroy,
    }

    public class Entity : MonoBehaviour
    {
        public EntityHandleAnimation anim;
        [SerializeField] protected EntityHandleInput entityHandleInput;
        [SerializeField] protected EntityHandleAttack handleAttack;
        [SerializeField] protected EntityHandleTakenDamage handleDamage;
        [SerializeField] protected EntityHandleMovement movementHandle;
        [SerializeField] protected EntityStatData entityStatData;
        // movement
        [SerializeField] protected Transform model;
        [SerializeField] protected float rotateSpeed;
        protected float currentMoveSpeed;
        protected float desMoveSpeed;
        protected float currentLockedTime;
        protected bool isLockedState;
        protected EntityState currentEntityState;
        protected EntityInput entityInput;

        protected virtual void Start()
        {
            entityInput = new EntityInput();
            if (handleDamage != null)
                handleDamage.Init(entityStatData.maxHealth, OnTakenDamage, OnDestroyed);
            if (movementHandle != null)
                movementHandle.Init(entityStatData);
            ChangeEntityState(EntityState.Entity_Idle);
        }

        protected virtual void Update()
        {
            // handle lock state
            if (isLockedState)
            {
                if (currentLockedTime > 0)
                    currentLockedTime -= Time.deltaTime;
                else
                {
                    OnFinishLockState();
                }

                return;
            }
            // get entity input
            GetInput();
            HandleAttackInput();
            if (!IsMovableState())
                return;
            Move(entityInput.moveVec);
        }

        protected void OnFinishLockState()
        {
            isLockedState = false;
            ChangeEntityState(EntityState.Entity_Idle);
        }

        protected virtual void Move(Vector3 moveVec)
        {
            ChangeEntityState(entityInput.moveVec != Vector3.zero ? EntityState.Entity_Move : EntityState.Entity_Idle);
            movementHandle.Move(moveVec);
        }

        protected void LateUpdate()
        {
            Rotate();
        }

        private bool IsMovableState()
        {
            return currentEntityState == EntityState.Entity_Idle
             || currentEntityState == EntityState.Entity_Move;
        }

        protected virtual void GetInput()
        {
            entityInput = entityHandleInput.GetInput();
        }

        protected virtual void Rotate()
        {
            movementHandle.Rotate(entityInput.moveVec);
        }

        protected virtual void CheckLockedState(EntityState entityState, float lockedTime = 0f)
        {
            isLockedState = IsLockState(entityState);
            if (isLockedState)
            {
                currentLockedTime = lockedTime;
            }
        }

        private bool IsLockState(EntityState entityState)
        {
            return entityState == EntityState.Entity_Attack_Short || entityState == EntityState.Entity_GetHit;
        }

        protected virtual void HandleAttackInput()
        {
            if (handleAttack != null)
                handleAttack.HandleAttackInput(this, entityInput);
        }

        public virtual void ChangeEntityState(EntityState newState, float lockedTime = 0f)
        {
            if (newState == currentEntityState)
                return;
            movementHandle.SetSpeedBaseOnState(newState);
            currentEntityState = newState;
            CheckLockedState(newState, lockedTime);
            switch (newState)
            {
                case EntityState.Entity_Idle:
                    PlayAnim(EntityAnimation.Character_Idle, 0.1f);
                    break;
                case EntityState.Entity_Move:
                    PlayAnim(EntityAnimation.Character_Run, 0.1f);
                    break;
                case EntityState.Entity_Attack_Short:
                    PlayAnim(EntityAnimation.Character_Attack, 0.1f);
                    break;
                case EntityState.Entity_Attack_Long:
                    // PlayAnim(EntityAnimation.Character_Idle);
                    PlayAnim(EntityAnimation.Character_StartCasting);
                    break;
                case EntityState.Entity_Block:
                    // PlayAnim(EntityAnimation.Character_Idle);
                    PlayAnim(EntityAnimation.Character_Block);
                    break;
                case EntityState.Entity_UnBlock:
                    PlayAnim(EntityAnimation.Character_UnBlock);
                    ChangeEntityState(EntityState.Entity_Idle);
                    break;
                case EntityState.Entity_GetHit:
                    PlayAnim(EntityAnimation.Character_GetHit);
                    break;
                case EntityState.Entity_Destroy:
                    PlayAnim(EntityAnimation.Character_Defeated);
                    break;
                default:
                    break;
            }
        }

        public void OnTakenDamage()
        {
            ChangeEntityState(EntityState.Entity_GetHit, 0.5f);
        }

        protected virtual void OnDestroyed()
        {
            ChangeEntityState(EntityState.Entity_Destroy);
        }

        public void PlayAnim(EntityAnimation animName, float transitionTime = 0f)
        {
            anim.PlayAnim(animName, transitionTime);
        }
    }
}
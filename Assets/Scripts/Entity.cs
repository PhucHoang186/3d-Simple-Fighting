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
        Entity_UnAttack_Long,
        Entity_Block,
        Entity_UnBlock,
        Entity_GetHit,
        Entity_Blocking_GetHit,
        Entity_Destroy,
        Entity_Attack_Deflected,
        Entity_Interact_With_UI,
    }

    public class Entity : MonoBehaviour
    {
        [SerializeField] protected EntityHandleAnimation anim;
        [SerializeField] protected EntityHandleInput entityHandleInput;
        [SerializeField] protected EntityHandleAttack handleAttack;
        [SerializeField] protected EntityHandleHealth handleHealth;
        [SerializeField] protected EntityHandleMovement handleMovement;
        [SerializeField] protected EntityStatData entityStatData;
        // movement
        [SerializeField] protected Transform model;
        protected float currentLockedTime;
        protected bool isLockedState;
        protected EntityState currentEntityState;
        protected EntityInput entityInput;

        protected virtual void Start()
        {
            if (handleHealth != null)
                handleHealth.InitActions(OnHit, OnDestroyed);

            if (handleMovement != null)
                handleMovement.Init(entityStatData);

            ChangeEntityState(EntityState.Entity_Idle);
        }

        private void OnDestroyed()
        {
            ChangeEntityState(EntityState.Entity_Destroy);
        }

        private void OnHit(float damageAmount)
        {
            ChangeEntityState(EntityState.Entity_GetHit, 1f);
        }

        protected virtual void Update()
        {
            if (IsInteractWithUI())
                return;
            if (IsDefeated())
                return;
            GetInput();
            // handle lock state
            if (CheckIfInLockState())
                return;
            // get entity input
            HandleAttackInput();
            if (!IsMovableState())
                return;
            Move(entityInput);
        }

        protected bool CheckIfInLockState()
        {
            if (!isLockedState)
                return false;

            if (currentLockedTime > 0)
                currentLockedTime -= Time.deltaTime;
            else
            {
                OnFinishLockState();
            }
            return true;
        }

        protected void OnFinishLockState()
        {
            isLockedState = false;
            ChangeEntityState(EntityState.Entity_Idle);
        }

        protected virtual void Move(EntityInput entityInput)
        {
            UpdateMoveStateAndAnimation();
            handleMovement.UpdateMoveSpeed(entityInput);
            handleMovement.Move(entityInput.moveVec);
        }

        private void UpdateMoveStateAndAnimation()
        {
            if (!entityInput.isHoldingCombatInput)
                ChangeEntityState(entityInput.moveVec != Vector3.zero ? EntityState.Entity_Move : EntityState.Entity_Idle);
        }

        protected void LateUpdate()
        {
            Rotate();
        }

        private bool IsMovableState()
        {
            return currentEntityState == EntityState.Entity_Idle
             || currentEntityState == EntityState.Entity_Move
             || currentEntityState == EntityState.Entity_Block
             || currentEntityState == EntityState.Entity_Attack_Long;
        }

        protected virtual void GetInput()
        {
            entityInput = entityHandleInput.GetInput();
        }

        protected virtual void Rotate()
        {
            handleMovement.Rotate(entityInput.lookRotation);
        }

        protected virtual void CheckLockedState(EntityState entityState, float lockedTime = 0f)
        {
            isLockedState = IsLockState(entityState);
            if (isLockedState)
                currentLockedTime = lockedTime;
        }

        private bool IsLockState(EntityState entityState)
        {
            return entityState == EntityState.Entity_Attack_Short
             || entityState == EntityState.Entity_GetHit
             || entityState == EntityState.Entity_Attack_Deflected;
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
            currentEntityState = newState;
            CheckLockedState(newState, lockedTime);
            anim.UpdateAnimationBaseOnState(newState);
            switch (newState)
            {
                case EntityState.Entity_Idle:
                    break;
                case EntityState.Entity_Move:
                    break;
                case EntityState.Entity_Attack_Short:
                    break;
                case EntityState.Entity_Attack_Long:
                    break;
                case EntityState.Entity_UnAttack_Long:
                    ChangeEntityState(EntityState.Entity_Idle);
                    break;
                case EntityState.Entity_Block:
                    break;
                case EntityState.Entity_UnBlock:
                    ChangeEntityState(EntityState.Entity_Idle);
                    break;
                case EntityState.Entity_GetHit:
                case EntityState.Entity_Blocking_GetHit:
                    break;
                case EntityState.Entity_Attack_Deflected:
                    break;
                case EntityState.Entity_Interact_With_UI:
                    break;
                case EntityState.Entity_Destroy:
                    break;
                default:
                    break;
            }
        }

        private bool IsInteractWithUI()
        {
            return currentEntityState == EntityState.Entity_Interact_With_UI;
        }

        protected bool IsDefeated()
        {
            return currentEntityState == EntityState.Entity_Destroy;
        }

        public void PlayAnim(EntityAnimation animName, float transitionTime = 0f)
        {
            anim.PlayAnim(animName, transitionTime);
        }
    }
}
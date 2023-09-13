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
        Entity_Idle,
        Entity_Move,
        Entity_Attack_Short,
        Entity_Attack_Long,
        Entity_Defend,
        Entity_GetHit,
        Entity_Destroy,
    }

    public class Entity : MonoBehaviour
    {
        public EntityHandleAnimation anim;
        [SerializeField] protected EntityStatData entityStatData;
        [SerializeField] protected EntityHandleInput entityHandleInput;
        [SerializeField] protected EntityHandleAttack handleAttack;
        [SerializeField] protected EntityHandleTakenDamage handleDamage;
        [SerializeField] protected Transform model;
        [SerializeField] protected LayerMask collideLayer;
        [SerializeField] protected float raycastDistance = 0.62f;
        protected float currentMoveSpeed;
        protected float currentLockedTime;
        protected bool isLockedState;
        protected Vector3 rotateVec;
        protected EntityState currentEntityState;
        protected EntityInput entityInput;

        protected virtual void Start()
        {
            currentMoveSpeed = entityStatData.movementSpeed;
            handleDamage.Init(entityStatData.maxHealth, OnTakenDamage, OnDestroyed);
            entityInput = new EntityInput();
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
                    isLockedState = false;
                    ChangeEntityState(EntityState.Entity_Idle);
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

        protected void LateUpdate()
        {
            Rotate();
        }

        private bool IsMovableState()
        {
            return currentEntityState == EntityState.Entity_Idle ||
             currentEntityState == EntityState.Entity_Move;
            //  currentEntityState == EntityState.Entity_Defend ||
            //  currentEntityState == EntityState.Entity_Attack_Long;
        }

        protected virtual void GetInput()
        {
            entityInput = entityHandleInput.GetInput();
        }

        protected virtual void Move(Vector3 moveVec)
        {
            if (Physics.Raycast(transform.position, Vector3.forward * moveVec.z, raycastDistance, collideLayer))// check back and forth
            {
                moveVec.z = 0f;
            }
            if (Physics.Raycast(transform.position, Vector3.right * moveVec.x, raycastDistance, collideLayer))// check left and right
            {
                moveVec.x = 0f;
            }
            var newPos = transform.position + moveVec.normalized * currentMoveSpeed;
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
        }

        protected virtual void Rotate()
        {
            rotateVec = Vector3.Lerp(rotateVec, entityInput.moveVec, entityStatData.rotateSpeed * Time.deltaTime);
            model.rotation = Quaternion.LookRotation(model.forward + rotateVec, Vector3.up);
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
            if (handleAttack == null)
                return;
            handleAttack.HandleAttackInput(this, entityInput);
        }

        public virtual void ChangeEntityState(EntityState newState, float lockedTime = 0f)
        {
            if (newState == currentEntityState)
                return;

            currentEntityState = newState;
            CheckLockedState(newState, lockedTime);
            anim.PlayEntityAnimState(currentEntityState);

            switch (currentEntityState)
            {
                case EntityState.Entity_Idle:
                    break;
                case EntityState.Entity_Move:
                    break;
                case EntityState.Entity_Attack_Short:
                    break;
                case EntityState.Entity_Attack_Long:
                case EntityState.Entity_Defend:
                    currentMoveSpeed = entityStatData.movementSpeed / 2f;
                    break;
                case EntityState.Entity_GetHit:
                    break;
                case EntityState.Entity_Destroy:
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
    }
}
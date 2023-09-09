using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Net.Security;

namespace Entity
{
    public enum EntityState
    {
        Entity_Idle,
        Entity_Move,
        Entity_Attack,
        Entity_GetHit,
        Entity_Destroy,
    }
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected EntityData entityData;
        [SerializeField] protected EntityCustomize entityCustomize;
        [SerializeField] protected EntityHandleAttack handleAttack;
        [SerializeField] protected float raycastDistance = 0.62f;
        [SerializeField] protected Transform model;

        protected float currentMoveSpeed;
        protected float currentHealth;
        protected float currentLockedTime;
        protected bool isLockedState;
        protected Vector3 rotateVec;
        protected EntityState currentEntityState;
        protected EntityInput entityInput;

        //property
        public float CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                // get hit
                if (value < currentHealth && value > 0)
                {
                    entityCustomize.PlayAnim(EntityAnimation.Character_GetHit);
                }
                currentHealth = value;
                if (currentHealth <= 0)
                {
                    BeingDestroyed();
                }
            }
        }

        protected virtual void Start()
        {
            currentMoveSpeed = entityData.movementSpeed;
            CurrentHealth = entityData.maxHealth;
            entityInput = new EntityInput();
        }

        protected virtual void TakeDamage(float damageAmount)
        {
            CurrentHealth -= damageAmount;
        }

        protected virtual void BeingDestroyed()
        {
            entityCustomize.PlayAnim(EntityAnimation.Character_Defeated);
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
                    // ChangeEntityState(EntityState.Entity_Idle, 0f);
                }

                return;
            }
            // get entity input
            GetInput();
            HandleAttackInput();
            if (IsNonMovableState())
                return;
            Move(entityInput.moveVec);
        }

        protected void LateUpdate()
        {
            Rotate();
        }

        private bool IsNonMovableState()
        {
            return currentEntityState != EntityState.Entity_Idle && currentEntityState != EntityState.Entity_Move;
        }

        protected virtual void Move(Vector3 moveVec)
        {
            if (Physics.Raycast(transform.position, Vector3.forward * moveVec.z, raycastDistance))// check back and forth
            {
                moveVec.z = 0f;
            }
            if (Physics.Raycast(transform.position, Vector3.right * moveVec.x, raycastDistance))// check left and right
            {
                moveVec.x = 0f;
            }
            var newPos = transform.position + moveVec.normalized * entityData.movementSpeed;
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
        }

        protected virtual void Rotate()
        {
            rotateVec = Vector3.Lerp(rotateVec, entityInput.moveVec, entityData.rotateSpeed * Time.deltaTime);
            model.rotation = Quaternion.LookRotation(model.forward + rotateVec, Vector3.up);
        }

        protected virtual void CheckLockedState(EntityState entityState, float lockedTime = 0f)
        {
            isLockedState = entityState == EntityState.Entity_Attack;
            if (isLockedState)
            {
                currentLockedTime = lockedTime;
            }
        }

        protected virtual void GetInput()
        {
        }

        protected virtual bool GetInstantAttackInput()
        {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        }

        protected virtual bool GetCastingAttackInput()
        {
            return Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R);
        }


        protected virtual void HandleAttackInput()
        {
            if (handleAttack == null)
                return;

            if (entityInput.isInstantAttackPressed || entityInput.isCastingAttackPressed)
            {

                ChangeEntityState(EntityState.Entity_Attack, 1f);
                handleAttack.HandleAttackInput(entityCustomize, entityInput.isInstantAttackPressed, entityInput.isCastingAttackPressed);
            }
        }

        protected virtual void ChangeEntityState(EntityState newState, float lockedTime = 0f)
        {
            if (newState == currentEntityState)
                return;

            currentEntityState = newState;
            CheckLockedState(newState, lockedTime);
            switch (currentEntityState)
            {
                case EntityState.Entity_Idle:
                    entityCustomize.PlayAnim(EntityAnimation.Character_Idle);
                    break;
                case EntityState.Entity_Move:
                    entityCustomize.PlayAnim(EntityAnimation.Character_Run);
                    break;
                case EntityState.Entity_Attack:
                    entityCustomize.PlayAnim(EntityAnimation.Character_Attack);
                    break;
                case EntityState.Entity_Destroy:
                    break;
                default:
                    break;
            }
        }
    }

    [Serializable]
    public class EntityInput
    {
        public Vector3 lookRotation;
        public Vector3 moveVec;
        public bool isInstantAttackPressed;
        public bool isCastingAttackPressed;
        public bool isLockTarget;
    }
}
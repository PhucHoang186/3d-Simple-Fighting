using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Entity
{
    public enum EntityState
    {
        Entity_Idle,
        Entity_Move,
        Entity_Attack,
        Entity_Destroy,
    }

    public class Entity : MonoBehaviour
    {
        [SerializeField] protected EntityData entityData;
        [SerializeField] protected EntityCustomize entityCustomize;
        [SerializeField] protected float raycastDistance = 0.62f;
        [SerializeField] protected float attackDuraction;
        [SerializeField] protected float rotateSpeed;
        [SerializeField] protected Transform model;

        protected float movementSpeed;
        protected float currentHealth;
        protected float currentLockedTime;
        protected bool isLockedState;
        protected Vector3 moveVec;
        protected Vector3 rotateVec;
        protected EntityState currentEntityState;
        //property
        public float CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                currentHealth = value;
                if (currentHealth <= 0)
                {
                    BeingDestroyed();
                }
            }
        }

        protected virtual void Start()
        {
            CurrentHealth = entityData.maxHealth;
        }

        protected virtual void TakeDamage(float damageAmount)
        {
            CurrentHealth -= damageAmount;
        }

        protected virtual void BeingDestroyed()
        {

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
                }

                return;
            }
            // get entity input
            GetInput();

            if (IsNonMovableState())
                return;

            Move();
            Rotate();
        }

        private bool IsNonMovableState()
        {
            return currentEntityState != EntityState.Entity_Idle && currentEntityState != EntityState.Entity_Move;
        }

        protected virtual void Move()
        {
            if (Physics.Raycast(transform.position, Vector3.forward * moveVec.z, raycastDistance))// check back and forth
            {
                moveVec.z = 0f;
            }
            if (Physics.Raycast(transform.position, Vector3.right * moveVec.x, raycastDistance))// check back and forth
            {
                moveVec.x = 0f;
            }
            var newPos = transform.position + moveVec.normalized * entityData.movementSpeed;
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
        }

        protected virtual void Rotate()
        {
            rotateVec = Vector3.Lerp(rotateVec, moveVec, rotateSpeed * Time.deltaTime);
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

        protected virtual bool GetAttackInput()
        {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        }

        protected virtual void Attack()
        {
            entityCustomize.PlayAnim(EntityAnimation.Character_Attack);
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
                    break;
                case EntityState.Entity_Destroy:
                    break;
                default:
                    break;

            }
        }
    }
}
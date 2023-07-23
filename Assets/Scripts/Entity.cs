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

        protected float maxHealth;
        protected float movementSpeed;
        protected float currentHealth;
        protected float currentLockedTime;
        protected bool isLockedState;
        protected Vector3 moveVec;
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
            maxHealth = entityData.maxHealth;
            CurrentHealth = maxHealth;
        }

        protected virtual void TakeDamage(float damageAmount)
        {
            CurrentHealth -= damageAmount;
        }

        protected virtual void BeingDestroyed()
        {

        }

        protected void Update()
        {
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
            GetInput();
            DetectAttack();

            if (IsNonMovableState()) return;
            Move(moveVec);
        }

        private bool IsNonMovableState()
        {
            return currentEntityState != EntityState.Entity_Idle && currentEntityState != EntityState.Entity_Move;
        }

        protected void Move(Vector3 moveVec)
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

        protected void CheckLockedState(EntityState entityState, float lockedTime = 0f)
        {
            isLockedState = entityState == EntityState.Entity_Attack;
            if (isLockedState)
            {
                currentLockedTime = lockedTime;
            }
        }

        private void GetInput()
        {
            moveVec.x = Input.GetAxisRaw("Horizontal");
            moveVec.z = Input.GetAxisRaw("Vertical");
            if (moveVec != Vector3.zero)
            {
                ChangeEntityState(EntityState.Entity_Move);
            }
            else
            {
                ChangeEntityState(EntityState.Entity_Idle);
            }
        }

        protected void DetectAttack()
        {
            if (GetAttackInput())
            {
                ChangeEntityState(EntityState.Entity_Attack, attackDuraction);
                Attack();
            }
        }

        private bool GetAttackInput()
        {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        }

        protected virtual void Attack()
        {
            entityCustomize.PlayAnim(EntityAnimation.Character_Attack);
        }

        protected void ChangeEntityState(EntityState newState, float lockedTime = 0f)
        {
            if(newState == currentEntityState)
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
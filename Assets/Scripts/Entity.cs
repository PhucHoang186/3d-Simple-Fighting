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


    public enum EntityAnimation
    {
        Character_Idle,
        Character_Roll,
        Character_Run,
        Character_Attack,
        Character_StartCasting,
        Character_Casting,
        Character_GetHit,
        Character_Defeated,
    }

    public class Entity : MonoBehaviour
    {
        [SerializeField] protected EntityStatData entityStatData;
        [SerializeField] protected EntityHandleInput entityHandleInput;
        [SerializeField] protected EntityHandleAttack handleAttack;
        [SerializeField] protected EntityHandleTakenDamage handleDamage;
        [SerializeField] protected Transform model;
        protected Animator anim;
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
            anim = GetComponent<Animator>();
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

        protected virtual void GetInput()
        {
            entityHandleInput.GetInput(entityInput);
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
            var newPos = transform.position + moveVec.normalized * entityStatData.movementSpeed;
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
            return entityState == EntityState.Entity_GetHit;
        }



        protected virtual void HandleAttackInput()
        {
            if (handleAttack == null)
                return;

            if (entityInput.StartAttack)
            {
                handleAttack.HandleAttackInput(this, entityInput);
            }
        }

        public virtual void ChangeEntityState(EntityState newState, float lockedTime = 0f)
        {
            if (newState == currentEntityState)
                return;

            currentEntityState = newState;
            CheckLockedState(newState, lockedTime);
            switch (currentEntityState)
            {
                case EntityState.Entity_Idle:
                    PlayAnim(EntityAnimation.Character_Idle);
                    break;
                case EntityState.Entity_Move:
                    PlayAnim(EntityAnimation.Character_Run);
                    break;
                case EntityState.Entity_Attack:
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
            PlayAnim(EntityAnimation.Character_GetHit);
        }

        protected virtual void OnDestroyed()
        {
            ChangeEntityState(EntityState.Entity_Destroy);
        }

        public void PlayAnim(EntityAnimation animName, float transitionTime = 0.1f)
        {
            anim.CrossFade(animName.ToString(), transitionTime);
        }
    }

    [Serializable]
    public class EntityInput
    {
        public Vector3 lookRotation;
        public Vector3 moveVec;
        public bool isInstantAttackPressed;
        public bool isCastingAttackPressed;
        public bool isCastingAttackReleased;
        public bool isLockTarget;
        public bool StartAttack => isInstantAttackPressed || isCastingAttackPressed || isCastingAttackReleased;
    }
}
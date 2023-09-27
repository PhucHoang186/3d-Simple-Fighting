using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Entity
{
    public class EntityHandleInput : MonoBehaviour
    {
        public virtual EntityInput GetInput()
        {
            var entityInput = new EntityInput();
            entityInput.moveVec = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
            // rotation
            entityInput.lookRotation.x = Input.GetAxis("Mouse X");
            entityInput.lookRotation.y = Input.GetAxis("Mouse Y");
            entityInput.lookRotation.Normalize();
            // attack
            entityInput.isInstantAttackPressed = GetInstantAttackInput();
            entityInput.isCastingAttackPressed = GetCastingAttackInput();
            entityInput.isCastingAttackReleased = GetCastingAttackReleaseInput();
            entityInput.isBlockPressed = GetBlockingInput();
            entityInput.isHoldingCombatInput = GetHoldingAttackInput();
            //lock target
            entityInput.isLockTarget = Input.GetKeyDown(KeyCode.LeftShift);
            return entityInput;
        }

        protected virtual bool GetInstantAttackInput()
        {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        }

        protected virtual bool GetCastingAttackInput()
        {
            return Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R);
        }

        protected virtual bool GetCastingAttackReleaseInput()
        {
            return Input.GetMouseButtonUp(1);
        }

        protected virtual bool GetHoldingAttackInput()
        {
            return Input.GetMouseButton(1);
        }

        protected virtual bool GetBlockingInput()
        {
            return Input.GetKey(KeyCode.Q);
        }
    }

    public struct EntityInput
    {
        public Vector3 lookRotation;
        public Vector3 moveVec;
        public bool isInstantAttackPressed;
        public bool isCastingAttackPressed;
        public bool isCastingAttackReleased;
        public bool isBlockPressed;
        public bool isLockTarget;
        public bool isHoldingCombatInput;
        public bool StartAttack => isInstantAttackPressed || isCastingAttackPressed || isCastingAttackReleased;
    }
}
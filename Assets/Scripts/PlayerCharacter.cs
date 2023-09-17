using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class PlayerCharacter : Entity
    {
        [Header("Player Settings")]
        [SerializeField] float lookSensitive;
        [Header("Player References")]
        [SerializeField] EntityWorldUI playerWorldUI;
        [SerializeField] Transform playerCamera;
        [SerializeField] float clampXRotationMin;
        [SerializeField] float clampXRotationMax;
        private Transform lockingTarget;
        private float xRotation;
        private float yRotation;

        protected override void Start()
        {
            base.Start();
            // Locks the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void Move(Vector3 moveVec)
        {
            ChangeEntityState(entityInput.moveVec != Vector3.zero ? EntityState.Entity_Move : EntityState.Entity_Idle);
            base.Move(moveVec);
        }

        protected override void Rotate()
        {
            if (lockingTarget != null)
            {
                var lookAtPoint = lockingTarget.position;
                lookAtPoint.y = model.position.y;
                model.LookAt(lookAtPoint);
            }
            else
            {
                xRotation = Mathf.Lerp(xRotation, xRotation - entityInput.lookRotation.y, Time.deltaTime * lookSensitive);
                yRotation = Mathf.Lerp(transform.eulerAngles.y, transform.eulerAngles.y + entityInput.lookRotation.x, Time.deltaTime * lookSensitive);
                xRotation = Mathf.Clamp(xRotation, clampXRotationMin, clampXRotationMax);
                playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

            }
        }
    }
}
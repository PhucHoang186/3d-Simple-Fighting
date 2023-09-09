using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Entity
{
    public class PlayerCharacter : Entity
    {
        [Header("Player Settings")]
        [SerializeField] float checkLocktargetRadius = 3f;
        [SerializeField] float lookSensitive;
        [SerializeField] string lockTargetObjLayerName;
        [Header("Player References")]
        [SerializeField] EntityWorldUI playerWorldUI;
        [SerializeField] Transform playerCamera;
        [SerializeField] float clampXRotationMin;
        [SerializeField] float clampXRotationMax;
        private bool isLockingToTarget;
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

        protected override void GetInput()
        {
            // movement
            base.GetInput();
            entityInput.moveVec = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
            // rotation
            entityInput.lookRotation.x = Input.GetAxis("Mouse X");
            entityInput.lookRotation.y = Input.GetAxis("Mouse Y");
            entityInput.lookRotation = entityInput.lookRotation.normalized;
            // attack
            entityInput.isInstantAttackPressed = GetInstantAttackInput();
            entityInput.isCastingAttackPressed = GetCastingAttackInput();
            //lock target
            entityInput.isLockTarget = Input.GetKeyDown(KeyCode.LeftShift);
        }

        protected override void Update()
        {
            base.Update();
            LockTarget();
        }

        protected override void Move(Vector3 moveVec)
        {
            EntityEvents.ON_CHANGE_ENTITY_STATE?.Invoke(entityInput.moveVec != Vector3.zero ? EntityState.Entity_Move : EntityState.Entity_Idle, 0.5f);
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
                xRotation = Mathf.Lerp(playerCamera.eulerAngles.x, playerCamera.eulerAngles.x - entityInput.lookRotation.y, Time.deltaTime * lookSensitive);
                yRotation = Mathf.Lerp(transform.eulerAngles.y, transform.eulerAngles.y + entityInput.lookRotation.x, Time.deltaTime * lookSensitive);
                playerCamera.eulerAngles = new Vector3(xRotation, playerCamera.eulerAngles.y, playerCamera.eulerAngles.z);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            }
        }

        private void LockTarget()
        {
            if (entityInput.isLockTarget)
            {
                if (isLockingToTarget)
                {
                    SetLockTarget(false);
                    return;
                }

                Collider[] colliders = Physics.OverlapSphere(transform.position, checkLocktargetRadius);
                if (colliders.Length > 0)
                {
                    foreach (var collider in colliders)
                    {
                        if (collider.gameObject.layer == LayerMask.NameToLayer(lockTargetObjLayerName))
                        {
                            SetLockTarget(true, collider.transform);
                            return;
                        }
                    }
                }
            }
        }

        private void SetLockTarget(bool isLockTarget, Transform target = null)
        {
            playerWorldUI.ToggleLockTargetUI(isLockTarget, target);
            isLockingToTarget = isLockTarget;
            lockingTarget = target;
        }
    }
}
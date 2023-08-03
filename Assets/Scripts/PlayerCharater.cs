using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class PlayerCharater : Entity
    {
        [Header("Player Settings")]
        [SerializeField] float checkLocktargetRadius = 3f;
        [SerializeField] float lookSensitive;
        [SerializeField] string lockTargetObjLayerName;
        [Header("Player References")]
        [SerializeField] EntityWorldUI playerWorldUI;
        [SerializeField] Transform playerCamera;
        private bool isLockingToTarget;
        private Transform lockingTarget;
        private Vector3 lookRotation;
        private float xRotation;
        private float yRotation;

        protected override void GetInput()
        {
            // movement
            base.GetInput();
            moveVec = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
            ChangeEntityState(moveVec != Vector3.zero ? EntityState.Entity_Move : EntityState.Entity_Idle);
            // rotation
            lookRotation.x = Input.GetAxis("Mouse X");
            lookRotation.y = Input.GetAxis("Mouse Y");
            lookRotation = lookRotation.normalized;

            // attack
            if (GetAttackInput())
            {
                ChangeEntityState(EntityState.Entity_Attack, attackDuraction);
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                LockTarget();
            }
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
                // xRotation = Mathf.Lerp(playerCamera.eulerAngles.x, playerCamera.eulerAngles.x + lookRotation.x, Time.deltaTime *  lookSensitive);
                yRotation = Mathf.Lerp(transform.eulerAngles.y, transform.eulerAngles.y + lookRotation.x, Time.deltaTime *  lookSensitive);
                // playerCamera.eulerAngles = new Vector3(xRotation, playerCamera.eulerAngles.y, playerCamera.eulerAngles.z);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            }
        }

        private void LockTarget()
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

        private void SetLockTarget(bool isLockTarget, Transform target = null)
        {
            playerWorldUI.ToggleLockTargetUI(isLockTarget, target);
            isLockingToTarget = isLockTarget;
            lockingTarget = target;
        }
    }
}
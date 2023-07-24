using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class PlayerCharater : Entity
    {
        [Header("Player Settings")]
        [SerializeField] float checkLocktargetRadius = 3f;
        [SerializeField] string lockTargetObjLayerName;
        [SerializeField] EntityWorldUI playerWorldUI;
        private bool isLockingToTarget;
        private Transform lockingTarget;

        protected override void GetInput()
        {
            // movement
            base.GetInput();
            moveVec.x = Input.GetAxisRaw("Horizontal");
            moveVec.z = Input.GetAxisRaw("Vertical");
            ChangeEntityState(moveVec != Vector3.zero ? EntityState.Entity_Move : EntityState.Entity_Idle);

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
                base.Rotate();
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
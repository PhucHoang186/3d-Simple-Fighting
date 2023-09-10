using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

public class EntityHandleInput : MonoBehaviour
{
    public void GetInput(EntityInput entityInput)
    {
        entityInput.moveVec = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        // rotation
        entityInput.lookRotation.x = Input.GetAxis("Mouse X");
        entityInput.lookRotation.y = Input.GetAxis("Mouse Y");
        entityInput.lookRotation.Normalize();
        // attack
        entityInput.isInstantAttackPressed = GetInstantAttackInput();
        entityInput.isCastingAttackPressed = GetCastingAttackInput();
        entityInput.isCastingAttackReleased = GetCastingAttackReleaseInput();
        if (GetCastingAttackReleaseInput())
            Debug.Log(GetCastingAttackReleaseInput());
        //lock target
        entityInput.isLockTarget = Input.GetKeyDown(KeyCode.LeftShift);
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
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Holding");
            return true;
            //  || !Input.GetKey(KeyCode.R);
        }
        return false;
    }
}

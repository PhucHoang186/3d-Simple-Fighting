using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform lookAtTarget;
    private Vector3 lookDir;


    void LateUpdate()
    {
        if (lookAtTarget == null)
            return;
        lookDir = lookAtTarget.position;
        transform.LookAt(lookDir);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform lookAtTarget;
    private Vector3 lookDir;

    void Start()
    {
        lookAtTarget = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (lookAtTarget == null)
            return;
        lookDir =  lookAtTarget.forward + transform.position;
        // lookDir.y = transform.position.y;
        transform.LookAt(lookDir);
    }
}

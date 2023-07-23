using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform lookAtTarget;


    void LateUpdate()
    {
        if(lookAtTarget == null)
            return;
        transform.LookAt(transform.position - lookAtTarget.position);
    }
}

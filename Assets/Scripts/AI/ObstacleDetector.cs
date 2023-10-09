using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using Unity.VisualScripting;
using System;

public class ObstacleDetector : MonoBehaviour, IDetect
{

    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float detectRange;
    [SerializeField] bool showGizmos;
    [SerializeField] float checkRepeatTime;
    private Collider[] colliders = new Collider[20];
    private int colliderFound;

    public void Detect(AIData aiData)
    {
        // may consider use overlapshere to prevent missing data
        Array.Clear(colliders, 0, colliders.Length);
        colliderFound = Physics.OverlapSphereNonAlloc(transform.position, detectRange, colliders, obstacleLayer);
        if (colliders != null)
            aiData.obstacles = colliders;
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos || !Application.isPlaying || colliders == null)
            return;
        Gizmos.color = Color.red;
        for (int i = 0; i < colliderFound; i++)
        {
            if (colliders[i] != null)
                Gizmos.DrawSphere(colliders[i].transform.position, 1f);
        }
    }
}

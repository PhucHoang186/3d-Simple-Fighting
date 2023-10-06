using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class ObstacleDetector : MonoBehaviour, IDetect
{

    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float detectRange;
    [SerializeField] bool showGizmos;
    private Collider[] colliders;

    public void Detect(AIData aiData)
    {
        colliders = Physics.OverlapSphere(transform.position, detectRange, obstacleLayer);
        if (colliders != null)
            aiData.colliders = colliders;
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos || !Application.isPlaying)
            return;
        Gizmos.color = Color.red;
        // Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using Unity.VisualScripting;

public class ObstacleDetector : MonoBehaviour, IDetect
{

    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float detectRange;
    [SerializeField] bool showGizmos;
    [SerializeField] float checkRepeatTime;
    private Collider[] colliders;

    public void Detect(AIData aiData)
    {
        colliders = Physics.OverlapSphere(transform.position, detectRange, obstacleLayer);
        if (colliders != null)
            aiData.colliders = colliders;
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos || !Application.isPlaying || colliders == null)
            return;
        Gizmos.color = Color.red;
        foreach (var collider in colliders)
        {
            Gizmos.DrawSphere(collider.transform.position, 1f);
        }
    }
}

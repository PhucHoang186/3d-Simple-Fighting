using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class TargetDetector : MonoBehaviour, IDetect
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float detectRange;
    [SerializeField] bool showGizmos;
    [SerializeField] float checkRepeatTime;
    private Collider[] colliders = new Collider[20];
    private int colliderFound;

    public void Detect(AIData aiData)
    {
        colliderFound = Physics.OverlapSphereNonAlloc(transform.position, detectRange, colliders, targetLayer);
        if (colliderFound > 0 && colliders != null)
        {
            var directionToTarget = (colliders[0].transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, detectRange, targetLayer))
            {
                aiData.targets = new List<Transform> { hit.transform };

            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos || !Application.isPlaying || colliders == null)
            return;
        Gizmos.color = Color.green;
        for (int i = 0; i < colliderFound; i++)
        {
            if (colliders[i] != null)
                Gizmos.DrawSphere(colliders[i].transform.position, 1f);
        }
    }
}

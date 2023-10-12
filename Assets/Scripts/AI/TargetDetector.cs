using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI;
using UnityEngine;

public class TargetDetector : MonoBehaviour, IDetect
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float detectRange = 20f;
    [SerializeField] bool showGizmos;
    [SerializeField] float checkRepeatTime = 0.1f;
    private Collider[] colliders = new Collider[100];
    private int colliderFound;

    public void Detect(AIData aiData)
    {
        colliderFound = Physics.OverlapSphereNonAlloc(transform.position, detectRange, colliders, targetLayer);
        if (colliderFound > 0 && colliders != null)
        {
            var directionToTarget = (colliders[0].transform.position - transform.position).normalized;
            Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, detectRange, obstacleLayer);
            if (hit.collider != null && (targetLayer & (1 << hit.collider.gameObject.layer)) != 0)
            {
                aiData.targets = new List<Transform> { hit.transform };
                return;
            }
        }
        aiData.targets = null;
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos || !Application.isPlaying)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using AI;
using Generation;
using UnityEngine;

public class SeekTargetBehaviour : MonoBehaviour, ISteering
{
    [SerializeField] float targetReachThreshold;
    [SerializeField] bool showGizmos;
    [SerializeField] bool reachTarget;
    private float[] interestsTempList;
    private Vector3 targetPositionCached;

    public (float[], float[]) GetSteering(float[] dangers, float[] interests, AIData aIData)
    {
        if (reachTarget)
        {

            if (aIData.targets == null || aIData.targets.Count <= 0)
            {
                aIData.currentTarget = null;
                return (dangers, interests);
            }
            else
            {
                reachTarget = false;
                aIData.currentTarget = aIData.targets[0];
            }

        }

        if (aIData.currentTarget != null && aIData.targets != null && aIData.targets.Contains(aIData.currentTarget))
        {
            targetPositionCached = aIData.currentTarget.transform.position;
        }


        if (Vector3.Distance(transform.position, targetPositionCached) <= targetReachThreshold)
        {
            aIData.currentTarget = null;
            reachTarget = true;
            return (dangers, interests);
        }
        Vector3 directionToTarget = (targetPositionCached - transform.position).normalized;

        var eightNormalizedDirectionsList = Direction3D.eightNormalizedDirectionsList;
        for (int i = 0; i < eightNormalizedDirectionsList.Count; i++)
        {
            var result = Vector3.Dot(directionToTarget, eightNormalizedDirectionsList[i]);
            if (result > 0)
            {
                if (interests[i] < result)
                {
                    interests[i] = result;
                }
            }
        }
        interestsTempList = interests;
        return (dangers, interests);
    }


    void OnDrawGizmosSelected()
    {
        if (!showGizmos || !Application.isPlaying || interestsTempList == null)
            return;
        Gizmos.color = Color.red;
        for (int i = 0; i < interestsTempList.Length; i++)
        {
            Gizmos.DrawRay(transform.position, interestsTempList[i] * Direction3D.eightNormalizedDirectionsList[i]);
        }
    }
}

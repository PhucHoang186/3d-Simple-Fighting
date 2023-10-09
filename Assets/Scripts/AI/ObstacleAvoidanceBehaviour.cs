using System.Collections;
using System.Collections.Generic;
using AI;
using Generation;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : MonoBehaviour, ISteering
{
    [SerializeField] float targetRadius;
    [SerializeField] float checkRadius;
    [SerializeField] bool showGizmos;
    private float[] dangerTempList;

    public (float[], float[]) GetSteering(float[] dangers, float[] interests, AIData aIData)
    {
        foreach (var obstacle in aIData.obstacles)
        {
            if (obstacle != null)
            {
                var directionToObstacle = obstacle.ClosestPoint(transform.position) - transform.position;
                var distanceToObstacle = directionToObstacle.magnitude;
                var weight = distanceToObstacle <= targetRadius ? 1 : (checkRadius - distanceToObstacle) / checkRadius;
                var directionToObstacleNormalized = directionToObstacle.normalized;
                var eightNormalizedDirectionsList = Direction3D.eightNormalizedDirectionsList;
                for (int i = 0; i < eightNormalizedDirectionsList.Count; i++)
                {
                    var result = Vector3.Dot(directionToObstacleNormalized, eightNormalizedDirectionsList[i]);
                    var finalValue = result * weight;
                    if (dangers[i] < finalValue)
                    {
                        dangers[i] = finalValue;
                    }
                }
            }
        }
        dangerTempList = dangers;

        return (dangers, interests);
    }

    void OnDrawGizmosSelected()
    {
        if (!showGizmos || !Application.isPlaying || dangerTempList == null)
            return;
        Gizmos.color = Color.red;
        for (int i = 0; i < dangerTempList.Length; i++)
        {
            Gizmos.DrawRay(transform.position, dangerTempList[i] * Direction3D.eightNormalizedDirectionsList[i]);
        }
    }
}

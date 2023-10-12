using System;
using System.Collections;
using System.Collections.Generic;
using Generation;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(ObstacleDetector), typeof(TargetDetector), typeof(ObstacleAvoidanceBehaviour))]
    // , typeof(SeekTargetBehaviour))]
    public class ContextSolver : MonoBehaviour
    {
        float[] dangers = new float[8];
        float[] interests = new float[8];
        float[] interestsGizmo = new float[8];
        Vector3 resultDirection = Vector3.zero;

        public Vector3 GetMovementDirection(ISteering[] steerings, AIData aIData)
        {
            Array.Clear(dangers, 0, dangers.Length);
            Array.Clear(interests, 0, interests.Length);
            foreach (var steering in steerings)
            {
                (dangers, interests) = steering.GetSteering(dangers, interests, aIData);
            }

            Vector3 outputDirection = Vector3.zero;
            for (int i = 0; i < 8; i++)
            {
                var result = Mathf.Clamp01(interests[i] - dangers[i]);
                outputDirection += Direction3D.eightNormalizedDirectionsList[i] * result;
                interestsGizmo[i] = result;
            }
            resultDirection = outputDirection.normalized;
            return resultDirection;
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDirection * 5f);
        }
    }
}
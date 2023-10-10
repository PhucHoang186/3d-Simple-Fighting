using System;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace AI
{
    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] protected float checkRange;
        [SerializeField] protected float chaseRange;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float attackSpeed;
        [SerializeField] protected ContextSolver solver;
        [SerializeField] protected Transform target;
        IDetect[] detectables;
        ISteering[] steerings;
        protected float currentAttackSpeed;
        private AIData aIData;
        private Vector3 movementDirection;

        public void Start()
        {
            target = FindAnyObjectByType<PlayerCharacter>().transform;
            detectables = GetComponents<IDetect>();
            steerings = GetComponents<ISteering>();
            aIData = new();
            InvokeRepeating(nameof(Detect), 0, 0.1f);
        }

        private void Detect()
        {
            foreach (var detectable in detectables)
            {
                detectable.Detect(aIData);
            }
            float[] dangers = new float[8];
            float[] interests = new float[8];
            movementDirection = solver.GetMovementDirection(steerings, aIData);
        }

        public EntityInput GetEnemyAIInput()
        {
            if (target == null)
                return new();
            var entityInput = new EntityInput();
            var distance = Vector3.Distance(transform.position, target.position);
            entityInput.moveVec = movementDirection;
            if (distance <= attackRange)
            {
                // entityInput = Attack();
            }
            return entityInput;
        }

        protected virtual EntityInput Attack()
        {
            var entityInput = new EntityInput();
            if (currentAttackSpeed <= 0)
            {
                entityInput.isInstantAttackPressed = true;
                currentAttackSpeed = attackSpeed;
            }
            else
            {
                entityInput.isInstantAttackPressed = false;
                currentAttackSpeed -= Time.deltaTime;
            }
            return entityInput;
        }
    }

    [Serializable]
    public class AIData
    {
        public List<Transform> targets;
        public Collider[] obstacles;
        public Transform currentTarget;
        public int TargetsCount => targets != null ? targets.Count : 0;
    }

    public interface IDetect
    {
        public void Detect(AIData aiData);
    }

    public interface ISteering
    {
        public (float[], float[]) GetSteering(float[] dangers, float[] interests, AIData aIData);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class EntityHandleTakenDamage : MonoBehaviour, IDamageable
    {
        [SerializeField] Entity entity;
        [SerializeField] protected ParticleSystem hitParticle;
        [SerializeField] protected Collider entityCollider;
        protected float currentHealth;
        public float MaxHealth { get; set; }

        public virtual void InitHealth(float maxHealth)
        {
            MaxHealth = maxHealth;
            currentHealth = MaxHealth;
        }

        // public void InitActions(Action onHit, Action onDestroy)
        // {
        //     this.onHitCb += onHit;
        //     this.onDestroyCb += onDestroy;
        // }

        public virtual void TakenDamage(float damageAmount = 1, Vector3 hitPoint = default(Vector3))
        {
            var particle = Instantiate(hitParticle, hitPoint, Quaternion.identity);
            Destroy(particle.gameObject, 2f);

            currentHealth -= damageAmount;
            if (currentHealth > 0)
            {
                OnHit(currentHealth);
            }
            else
            {
                OnDestroyed();
            }
        }

        protected virtual void OnHit(float currentHealth)
        {
            entity.ChangeEntityState(EntityState.Entity_GetHit, 1f);
            // onHitCb?.Invoke();
        }

        protected virtual void OnDestroyed()
        {
            entity.ChangeEntityState(EntityState.Entity_Destroy);
            Destroy(entityCollider);
            // onDestroyCb?.Invoke();
        }

    }
}
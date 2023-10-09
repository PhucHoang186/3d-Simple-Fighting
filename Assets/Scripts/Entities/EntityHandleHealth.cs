using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Entity
{
    public class EntityHandleHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] EntityStatData entityStatData;
        [SerializeField] protected ParticleSystem hitParticle;
        [SerializeField] protected Collider entityCollider;
        protected float currentHealth;
        protected float nullifyAmount;
        protected Action<float, float> onChangeHealthCb;
        protected Action onDestroyCb;
        public float MaxHealth { get; set; }

        void Awake()
        {
            InitHealth(entityStatData.maxHealth);
        }

        public virtual void InitHealth(float maxHealth)
        {
            MaxHealth = maxHealth;
            currentHealth = MaxHealth;
        }

        public void InitActions(Action<float, float> onHit, Action onDestroy)
        {
            this.onChangeHealthCb += onHit;
            this.onDestroyCb += onDestroy;
        }

        public virtual void TakenDamage(float damageAmount = 1, Vector3 hitPoint = default)
        {
            var remainingDamageDealt = damageAmount - nullifyAmount;
            if (remainingDamageDealt <= 0)
                return;

            currentHealth -= remainingDamageDealt;
            OnChangeHealth(currentHealth);
            
            SpawnHitVfx(hitPoint);
            if (currentHealth <= 0)
                OnDestroyed();
        }

        private void SpawnHitVfx(Vector3 hitPoint)
        {
            var particle = Instantiate(hitParticle, hitPoint, Quaternion.identity);
            Destroy(particle.gameObject, 2f);
        }

        public void Heal(int healAmount)
        {
            currentHealth += healAmount;
            OnChangeHealth(healAmount);
        }

        public void OnChangeHealth(float currentHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0 , MaxHealth);
            onChangeHealthCb?.Invoke(currentHealth, MaxHealth);
        }

        public void OnDestroyed()
        {
            Destroy(entityCollider);
            onDestroyCb?.Invoke();
        }

        public void OnDestroy()
        {
            onChangeHealthCb = null;
            onDestroyCb = null;
        }
    }
}
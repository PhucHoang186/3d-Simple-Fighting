using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandleTakenDamage : MonoBehaviour, IDamageable
{
    public Action onHitCb;
    public Action onDestroyCb;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private Collider entityCollider;
    private float currentHealth;

    public void Init(float maxHealth, Action onHit = null, Action onDestroy = null)
    {
        currentHealth = maxHealth;
        this.onHitCb = onHit;
        this.onDestroyCb = onDestroy;
    }

    public void TakenDamage(float damageAmount = 1, Vector3 hitPoint = default(Vector3))
    {
        var particle = Instantiate(hitParticle, hitPoint, Quaternion.identity);
        Destroy(particle.gameObject, 2f);

        currentHealth -= damageAmount;
        if (currentHealth > 0)
        {
            onHitCb?.Invoke();
        }
        else
        {
            Destroy(entityCollider);
            onDestroyCb?.Invoke();
        }
    }

    private void OnDestroy()
    {
        onHitCb = null;
        onDestroyCb = null;
    }
}

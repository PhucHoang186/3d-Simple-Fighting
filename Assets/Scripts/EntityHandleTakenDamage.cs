using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandleTakenDamage : MonoBehaviour, IDamageable
{
    public Action ON_HIT;
    public Action ON_DESTROY;
    [SerializeField] private ParticleSystem hitParticle;
    private float currentHealth;
    private bool isDestroyed;

    public void Init(float maxHealth, Action onHit = null, Action onDestroy = null)
    {
        currentHealth = maxHealth;
        this.ON_HIT = onHit;
        this.ON_DESTROY = onDestroy;
    }

    public void TakenDamage(float damageAmount = 1, Vector3 hitPoint = default(Vector3))
    {
        if (isDestroyed)
            return;

        var particle = Instantiate(hitParticle, hitPoint, Quaternion.identity);
        Destroy(particle.gameObject, 2f);

        currentHealth -= damageAmount;
        if (currentHealth > 0)
        {
            ON_HIT?.Invoke();
        }
        else
        {
            isDestroyed = true;
            ON_DESTROY?.Invoke();
        }
    }

    private void OnDestroy()
    {
        ON_HIT = null;
        ON_DESTROY = null;
    }
}

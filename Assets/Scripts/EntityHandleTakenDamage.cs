using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

public class EntityHandleTakenDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem hitParticle;
    private Entity.Entity entity;

    void Start()
    {
        entity = GetComponent<Entity.Entity>();
    }

    public void TakenDamage(float damageAmount = 1, Vector3 hitPoint = default(Vector3))
    {
        if (entity == null || entity.CurrentHealth <= 0)
            return;

        entity.CurrentHealth -= damageAmount;
        var particle = Instantiate(hitParticle, hitPoint, Quaternion.identity);
        Destroy(particle.gameObject, 2f);
    }
}

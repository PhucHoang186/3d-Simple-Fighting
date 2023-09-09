using UnityEngine;

public interface IDamageable
{
    public void TakenDamage(float damageAmount = 1, Vector3 hitPoint = default(Vector3));
}

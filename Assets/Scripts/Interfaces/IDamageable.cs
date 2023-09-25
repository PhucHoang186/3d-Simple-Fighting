using UnityEngine;

public interface IDamageable
{
    public void TakenDamage(float damageAmount, Vector3 hitPoint = default);
}

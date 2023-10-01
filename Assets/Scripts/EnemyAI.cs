using Entity;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float checkRange;
    [SerializeField] protected float chaseRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected Transform target;
    protected float currentAttackSpeed;

    public void Start()
    {
        target = FindAnyObjectByType<PlayerCharacter>().transform;
    }

    public EntityInput GetEnemyAIInput()
    {
        if (target == null)
            return new();
        var entityInput = new EntityInput();
        var distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            entityInput = Attack();
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
        // entityInput.isBlockPressed = true;
        return entityInput;
    }
}

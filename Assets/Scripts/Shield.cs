using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Equipment
{
    [SerializeField] Collider shieldCollider;
    [SerializeField] float physicDefend;
    [SerializeField] float magicDefend;
    [SerializeField] GameObject sparkle;
    [SerializeField] float blockCooldown;
    private float currentBlockCooldown;
    private bool canBlock;

    public void ToggleShieldHitBox(bool isActive)
    {
        shieldCollider.enabled = isActive;
    }

    public void Init(Entity.Entity entity)
    {
        // this.entity1 = entity;
    }

    private void Update()
    {
        if (currentBlockCooldown > 0)
            currentBlockCooldown -= Time.deltaTime;
        else
        {
            canBlock = true;
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (!canBlock)
            return;

        if (collider.TryGetComponent<Weapon>(out Weapon weapon))
        {
            var entity = weapon.GetComponentInParent<Entity.Entity>();
            entity.ChangeEntityState(Entity.EntityState.Entity_Attack_Deflected, 1f);
            ShowHitSparkle();
            currentBlockCooldown = blockCooldown;
            canBlock = false;
            return;
        }

        if (collider.TryGetComponent<Spell>(out Spell spell))
        {
            // spell
        }
    }

    private void ShowHitSparkle()
    {
        StartCoroutine(CorShowHitSparkle());
    }

    private IEnumerator CorShowHitSparkle()
    {
        sparkle.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        sparkle.SetActive(false);
    }
}

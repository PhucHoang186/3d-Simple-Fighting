using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] GameObject hitVfx;
    [SerializeField] float raycastDistance;
    private Action<IDamageable, Vector3> OnHitTargetCb;
    private Vector3 shootDirection;
    private float spellSpeed;
    private bool isStartCastSpell;
    private bool isCastedSpell;
    private float currentTime;
    private float desCastTime;

    public void Init(float castTime, float speed = 0f, Action<IDamageable, Vector3> OnHitTarget = null)
    {
        spellSpeed = speed;
        this.OnHitTargetCb = OnHitTarget;
        SetUpSpellCasting(castTime);
    }

    private void SetUpSpellCasting(float castTime)
    {
        isStartCastSpell = true;
        currentTime = 0f;
        desCastTime = castTime;
    }

    public virtual void ActivateSkill()
    {
        shootDirection = CalculateDirection();
        isCastedSpell = true;
        transform.SetParent(null);
        Destroy(gameObject, 5f);
    }

    public virtual void DeActivateSkill()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (isStartCastSpell)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.one * currentTime / desCastTime;
            if (currentTime >= desCastTime)
                isStartCastSpell = false;
        }

        if (!isCastedSpell)
            return;
        MoveToTarget();
        if (Physics.Raycast(transform.position, shootDirection, out RaycastHit hit, raycastDistance))
        {
            OnHit();
        }
    }

    private void MoveToTarget()
    {
        transform.Translate(spellSpeed * Time.deltaTime * shootDirection, Space.World);
    }

    private Vector3 CalculateDirection()
    {
        var ray = CameraController.Instance.mainCam.ViewportPointToRay(Vector2.one * 0.5f);
        var direction = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
            direction = hit.point - transform.position;
        else
            direction = ray.GetPoint(75) - transform.position;

        direction = direction.normalized;
        return direction;
    }

    private void OnHit()
    {
        isCastedSpell = false;
        model.SetActive(false);
        hitVfx.SetActive(true);
        Destroy(gameObject, 3f);
        Collider[] colliders = new Collider[10];
        Physics.OverlapSphereNonAlloc(transform.position, 2f, colliders);
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider != null && collider.transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    OnHitTargetCb?.Invoke(damageable, collider.transform.position);
                }
            }
        }
    }
}

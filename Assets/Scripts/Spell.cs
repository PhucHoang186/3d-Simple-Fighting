using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] GameObject hitVfx;
    [SerializeField] float raycastDistance;
    private Vector3 shootDirection;
    private float spellSpeed;
    private bool isCastedSpell;

    public void Init(float speed = 0f)
    {
        spellSpeed = speed;
    }

    public virtual void CastSpell()
    {
        shootDirection = CalculateDirection(); ;
        transform.SetParent(null);
        isCastedSpell = true;
        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        if (!isCastedSpell)
            return;
        MoveToTarget();
        if (Physics.Raycast(transform.position, shootDirection, raycastDistance))
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
        var direction = ray.GetPoint(75) - transform.position;
        direction = direction.normalized;
        return direction;
    }

    private void OnHit()
    {
        model.SetActive(false);
        hitVfx.SetActive(true);
        Destroy(gameObject, 3f);
    }
}

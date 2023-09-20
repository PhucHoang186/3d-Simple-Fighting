using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment
{
    [SerializeField] Transform mainPart;
    [SerializeField] Transform leftPart;
    [SerializeField] Transform rightPart;
    public float DefendStat { get; set; }

    public void EquipMultipleParts(Transform mainPart, Equipment oldEquipment, Transform leftPart = null, Transform rightPart = null)
    {
        if (oldEquipment != null)
        {
            Destroy(oldEquipment.gameObject);
        }

        this.mainPart.parent = mainPart;
        this.mainPart.ResetTransform();

        this.leftPart.parent = leftPart;
        this.leftPart.ResetTransform();

        this.rightPart.parent = rightPart;
        this.rightPart.ResetTransform();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomObjectVariant : MonoBehaviour
{
    [SerializeField] Transform modelContainer;

    public void SpawnObject(GameObject objPrefab)
    {
        Instantiate(objPrefab, modelContainer);
    }
}

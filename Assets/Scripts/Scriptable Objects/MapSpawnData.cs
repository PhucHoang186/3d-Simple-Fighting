using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map Data")]
public class MapSpawnData : ScriptableObject
{
    [Header("Params")]
    [Range(0f, 1f)] public float spawnFloorVariantsPercent;
    [Range(0f, 1f)] public float spawnWallVariantsPercent;
    [Range(0f, 1f)] public float spawnCeilingVariantsPercent;

    [Space(10)]
    [Header("Floors")]
    public GameObject mainFloorPrefab;
    public List<GameObject> floorVariants;

    [Space(10)]
    [Header("Ceiling")]
    public GameObject mainCeilingPrefab;
    public List<GameObject> ceilingVariants;

    [Space(10)]
    [Header("Walls")]
    public List<GameObject> wallTilePrefabs;
    public List<GameObject> cornerTilePrefabs;


    public GameObject GetRandomFloorPrefab()
    {
        return GetRandomPrefab(mainFloorPrefab, floorVariants, spawnFloorVariantsPercent);
    }

    public GameObject GetRandomCeilingPrefab()
    {
        return GetRandomPrefab(mainCeilingPrefab, ceilingVariants, spawnCeilingVariantsPercent);
    }

    public GameObject GetRandomPrefab(GameObject mainPrefab, List<GameObject> prefabVariants, float variantSpawnPercent = 1f)
    {
        var spawnPercent = Random.Range(0f, 1f);
        if (spawnPercent <= variantSpawnPercent)
        {
            return prefabVariants[Random.Range(0, prefabVariants.Count)];
        }
        return mainPrefab;
    }
    
}

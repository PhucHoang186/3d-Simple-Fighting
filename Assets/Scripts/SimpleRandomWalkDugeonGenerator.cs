using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using NaughtyAttributes;

public class SimpleRandomWalkDugeonGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MapSpawner mapSpawner;
    [Header("params")]
    [SerializeField] SimpleRandomWalkData simpleRandomWalkData;
    [SerializeField] protected Vector3Int startPosition = Vector3Int.zero;
    [SerializeField] private bool startRandomlyEachInteraction = true;

    [Button]
    public void RunProceduralGeneration()
    {
        HashSet<Vector3Int> floorPositions = RunRandomWalk();
        mapSpawner.DestroyTiles();
        mapSpawner.SpawnFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, mapSpawner, simpleRandomWalkData.stepOffset, simpleRandomWalkData.wallLayer);
    }

    [Button]
    public void ClearMap()
    {
        mapSpawner.DestroyTiles();
    }

    protected HashSet<Vector3Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        for (int i = 0; i < simpleRandomWalkData.interations; i++)
        {
            var path = ProceduralGenrationAlgorithms.SimpleRandomWalk(currentPosition, simpleRandomWalkData.walkLength, simpleRandomWalkData.stepOffset);
            floorPositions.UnionWith(path);
            if (startRandomlyEachInteraction)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }
}

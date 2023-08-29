using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using NaughtyAttributes;

public class SimpleRandomWalkDugeonGenerator : MonoBehaviour
{
    [SerializeField] protected Vector3Int startPosition = Vector3Int.zero;
    [SerializeField] private int interations = 10;
    [SerializeField] private int walkLength = 10;
    [SerializeField] private bool startRandomlyEachInteraction = true;

    [Button]    
    public void RunProceduralGeneration()
    {
        HashSet<Vector3Int> floorPositions = RunRandomWalk();
        foreach (var position in floorPositions)
        {
            Debug.Log(position);
        }
    }

    protected HashSet<Vector3Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        for (int i = 0; i < interations; i++)
        {
            var path = ProceduralGenrationAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path);
            if(startRandomlyEachInteraction)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }
}

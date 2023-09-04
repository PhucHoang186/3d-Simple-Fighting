using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenrationAlgorithms
{
    public static HashSet<Vector3Int> SimpleRandomWalk(Vector3Int startPosition, int walkLength, int stepOffset)
    {
        HashSet<Vector3Int> path = new HashSet<Vector3Int>();
        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + stepOffset * Direction3D.GetRandomCardinalDirection(false);
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }


}

public static class Direction3D
{
    public enum DirectionType
    {
        Forward,
        Back,
        Right,
        Left,
        Up,
        Down,
    }

    public static List<Vector3Int> cardinalDirectionsList = new List<Vector3Int>
    {
        Vector3Int.forward,
        Vector3Int.back,
        Vector3Int.right,
        Vector3Int.left,
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.forward + Vector3Int.right,
        Vector3Int.forward + Vector3Int.left,
        Vector3Int.back + Vector3Int.right,
        Vector3Int.back + Vector3Int.left,
    };

    public static Vector3Int GetRandomCardinalDirection(bool isThreeDemension = true)
    {
        var length = isThreeDemension ? cardinalDirectionsList.Count : cardinalDirectionsList.Count - 6;
        return cardinalDirectionsList[Random.Range(0, length)];
    }

    public static List<Vector3Int> GetCardinalDirectionsListIgnoreY()
    {
        return cardinalDirectionsList.GetRange(0, 4);
    }

    public static List<Vector3Int> GetExtraDirectionList()
    {
        return cardinalDirectionsList.GetRange(6, 4);
    }


}

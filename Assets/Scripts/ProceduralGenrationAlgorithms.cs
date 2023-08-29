using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenrationAlgorithms
{
    public static HashSet<Vector3Int> SimpleRandomWalk(Vector3Int startPosition, int walkLength)
    {
        HashSet<Vector3Int> path = new HashSet<Vector3Int>();
        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction3D.GetRandomCardinalDirection(false);
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }


}

public static class Direction3D
{
    public static List<Vector3Int> cardinalDirectioonList = new List<Vector3Int>
    {
        Vector3Int.forward,
        Vector3Int.back,
        Vector3Int.right,
        Vector3Int.left,
        Vector3Int.up,
        Vector3Int.down,
    };

    public static Vector3Int GetRandomCardinalDirection(bool isThreeDemension = true)
    {
        var length = isThreeDemension ? cardinalDirectioonList.Count : cardinalDirectioonList.Count - 2;
        return cardinalDirectioonList[Random.RandomRange(0, length)];
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Generation;
using UnityEngine;

public class RandomWalkGeneration : MonoBehaviour
{
    int stepOffset;
    SimpleRandomWalkData simpleRandomWalkData;
    protected Vector3Int startPosition;
    int corridorLength;
    int corridorInteration;
    bool randomWalkRoom;
    float roomPercent;
    int roomWidth;
    int roomHeight;

    public void Init(Vector3Int startPosition, int stepOffset, float roomPercent, bool isRandomWalk, SimpleRandomWalkData randomWalkData, int corridorLength, int corridorInteration, int width, int height)
    {
        this.stepOffset = stepOffset;
        this.randomWalkRoom = isRandomWalk;
        this.roomPercent = roomPercent;
        this.simpleRandomWalkData = randomWalkData;
        this.startPosition = startPosition;
        this.corridorLength = corridorLength;
        this.corridorInteration = corridorInteration;
        this.roomWidth = width;
        this.roomHeight = height;
    }

    public (HashSet<Vector3Int>, HashSet<Vector3Int>) CreateRoomsAndCorridors()
    {
        HashSet<Vector3Int> roomPositions = new();
        HashSet<Vector3Int> deadEndPositions = new();
        HashSet<Vector3Int> normalEndPositions = new();
        HashSet<Vector3Int> corridorPositions = CreateCorridorsAndGetPotentialRoomPositions(deadEndPositions, normalEndPositions);
        List<Vector3Int> roomsToCreate = GetRoomNumberToSpawn(deadEndPositions, normalEndPositions);

        if (randomWalkRoom)
            roomPositions.UnionWith(CreateRoomsFromPosition(roomsToCreate));
        else
            roomPositions.UnionWith(CreateRectangleRoom(roomWidth, roomHeight, roomsToCreate));
        return (roomPositions, corridorPositions);
    }

    protected HashSet<Vector3Int> CreateCorridorsAndGetPotentialRoomPositions(HashSet<Vector3Int> deadEndRoomsPositions, HashSet<Vector3Int> normalRoomPositions)
    {
        HashSet<Vector3Int> corridorPositions = new();
        HashSet<Vector3Int> allRoomsPositions = new();
        var currentPosition = startPosition;
        deadEndRoomsPositions.Add(currentPosition);
        for (int i = 0; i < corridorInteration; i++)
        {
            // get corridor positions
            var path = ProceduralGenrationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength, stepOffset);
            corridorPositions.UnionWith(path);
            currentPosition = path[^1];
            allRoomsPositions.Add(currentPosition);
        }

        foreach (var roomPosition in allRoomsPositions)
        {
            // detect dead end points
            int neighborNumber = 0;
            var directionList = Direction3D.GetCardinalDirectionsListIgnoreY();
            for (int j = 0; j < directionList.Count; j++)
            {
                if (corridorPositions.Contains(roomPosition + stepOffset * directionList[j]))
                {
                    neighborNumber++;
                }
            }
            if (neighborNumber == 1)
                deadEndRoomsPositions.Add(roomPosition);
            else
                normalRoomPositions.Add(roomPosition);
        }

        return corridorPositions;
    }

    public HashSet<Vector3Int> CreateRectangleRoom(int width, int height, List<Vector3Int> roomCenterPositions)
    {
        HashSet<Vector3Int> floorPositions = new();
        foreach (var roomCenter in roomCenterPositions)
        {
            for (int i = -width / 2; i < width / 2; i++)
            {
                for (int j = -height / 2; j < height / 2; j++)
                {
                    var floorPosition = roomCenter + new Vector3Int(i, 0, j) * stepOffset;
                    floorPositions.Add(floorPosition);
                }
            }
        }
        return floorPositions;
    }

    public List<Vector3Int> GetRoomNumberToSpawn(HashSet<Vector3Int> deadEndPositions, HashSet<Vector3Int> normalEndPositions)
    {
        // Shuffle
        int roomToCreateCount = Mathf.RoundToInt(normalEndPositions.Count * roomPercent);
        var potentialRoomList = normalEndPositions.ToList();
        potentialRoomList.Shuffle();
        List<Vector3Int> roomsToCreate = potentialRoomList.GetRange(0, roomToCreateCount);
        roomsToCreate.AddRange(deadEndPositions);
        return roomsToCreate;
    }

    public HashSet<Vector3Int> CreateRoomsFromPosition(List<Vector3Int> roomsToCreate)
    {
        HashSet<Vector3Int> roomFloors = new();
        foreach (var roomPosition in roomsToCreate)
        {
            roomFloors.UnionWith(CreateRoomFromPosition(roomPosition));

        }
        return roomFloors;
    }

    protected HashSet<Vector3Int> CreateRoomFromPosition(Vector3Int startPosition)
    {
        var currentPosition = startPosition;

        HashSet<Vector3Int> floorPositions = new();
        for (int i = 0; i < simpleRandomWalkData.interations; i++)
        {
            var path = ProceduralGenrationAlgorithms.SimpleRandomWalk(currentPosition, simpleRandomWalkData.walkLength, stepOffset);
            floorPositions.UnionWith(path);
            if (simpleRandomWalkData.startRandomlyEachInteraction)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }
}

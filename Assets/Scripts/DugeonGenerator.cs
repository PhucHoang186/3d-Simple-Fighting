using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using TMPro;
using Unity.Mathematics;

public class DugeonGenerator : MonoBehaviour
{
    [SerializeField] int stepOffset;
    [SerializeField] int wallLayer;
    [Header("Random Walk Procedural")]
    [SerializeField] MapSpawner mapSpawner;
    [SerializeField] SimpleRandomWalkData simpleRandomWalkData;
    [SerializeField] protected Vector3Int startPosition = Vector3Int.zero;
    [SerializeField] int corridorLength;
    [SerializeField] int corridorInteration;
    [Range(0f, 1)]
    [SerializeField] float roomPercent;
    [Header("Binary Partition Procedural")]
    [SerializeField] int dungeonWidth;
    [SerializeField] int dungeonHeight;
    [SerializeField] int minWidth;
    [SerializeField] int minHeight;
    [SerializeField] int offset;
    [SerializeField] bool randomWalkRoom;

    [Button]
    public void RunRandomWalkGeneration()
    {
        HashSet<Vector3Int> floorPositions = CreateRoomsAndCorridors();
        mapSpawner.DestroyTiles();
        mapSpawner.SpawnFloorTiles(floorPositions, wallLayer);
        WallGenerator.CreateWalls(floorPositions, mapSpawner, stepOffset, wallLayer);
    }

    [Button]
    public void RunBinaryPartitioningGeneration()
    {
        var roomList = ProceduralGenrationAlgorithms.BinaryPartitioning(new BoundsInt(startPosition, new Vector3Int(dungeonWidth * stepOffset, 0, dungeonHeight * stepOffset)), minWidth * stepOffset, minHeight * stepOffset, stepOffset);
        HashSet<Vector3Int> corridorPositions = CreateCorridorsConnectRooms(roomList);
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        if (randomWalkRoom)
        {
            floorPositions = CreateRoomsRandomly(roomList);
        }
        else
        {
            floorPositions = CreateRoomsFromList(roomList);
        }

        floorPositions.UnionWith(corridorPositions);
        mapSpawner.DestroyTiles();
        mapSpawner.SpawnFloorTiles(floorPositions, wallLayer);
        WallGenerator.CreateWalls(floorPositions, mapSpawner, stepOffset, wallLayer);
    }

    [Button]
    public void ClearMap()
    {
        mapSpawner.DestroyTiles();
    }

    private HashSet<Vector3Int> CreateCorridorsConnectRooms(List<BoundsInt> roomList)
    {
        List<Vector3Int> roomCenters = new List<Vector3Int>();
        HashSet<Vector3Int> corridors = new HashSet<Vector3Int>();
        foreach (var room in roomList)
        {
            roomCenters.Add(Vector3Int.RoundToInt(room.center));
        }
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);
        while (roomCenters.Count > 0)
        {
            Vector3Int closestRoomCenter = FindClosetRoomCenter(currentRoomCenter, roomCenters);
            var corridor = CreateCorridorBetweenTwoRoomsCenter(currentRoomCenter, closestRoomCenter);
            currentRoomCenter = closestRoomCenter;
            roomCenters.Remove(currentRoomCenter);
            corridors.UnionWith(corridor);
        }
        return corridors;
    }

    private HashSet<Vector3Int> CreateCorridorBetweenTwoRoomsCenter(Vector3Int currentRoomCenter, Vector3Int closestRoomCenter)
    {
        HashSet<Vector3Int> corridor = new HashSet<Vector3Int>();
        var position = currentRoomCenter;
        position = ConvertPositionTo_Gcd_OfStepOffset(position);
        closestRoomCenter = ConvertPositionTo_Gcd_OfStepOffset(closestRoomCenter);

        while (position.z != closestRoomCenter.z)
        // while (position.z > closestRoomCenter.z + stepOffset && position.z < closestRoomCenter.z - offset)
        {
            if (position.z > closestRoomCenter.z)
                position += Vector3Int.back * stepOffset;
            else if (position.z < closestRoomCenter.z)
                position += Vector3Int.forward * stepOffset;

            // if (position.z % stepOffset == 0)
                corridor.Add(position);
        }


        while (position.x != closestRoomCenter.x)
        // while (position.x > closestRoomCenter.x + stepOffset && position.x < closestRoomCenter.x - offset)
        {
            if (position.x > closestRoomCenter.x)
                position += Vector3Int.left * stepOffset;
            else if (position.x < closestRoomCenter.x)
                position += Vector3Int.right * stepOffset;
            // if (position.x % stepOffset == 0)
                corridor.Add(position);
        }

        return corridor;
    }

    private Vector3Int FindClosetRoomCenter(Vector3Int currentRoomCenter, List<Vector3Int> roomCenters)
    {
        var closestRoomCenter = Vector3Int.zero;
        var minDistance = Mathf.Infinity;
        foreach (var roomCenter in roomCenters)
        {
            var distance = Vector3.Distance(roomCenter, currentRoomCenter);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRoomCenter = roomCenter;
            }
        }
        return closestRoomCenter;
    }

    private HashSet<Vector3Int> CreateRoomsAndCorridors()
    {
        HashSet<Vector3Int> roomPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> deadEndPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> normalEndPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> corridorPositions = CreateCorridorsAndGetPotentialRoomPositions(deadEndPositions, normalEndPositions);
        List<Vector3Int> roomsToCreate = GetRoomNumberToSpawn(deadEndPositions, normalEndPositions);

        CreateRoomsFromPosition(roomPositions, roomsToCreate);
        roomPositions.UnionWith(corridorPositions);
        return roomPositions;
    }

    private Vector3Int ConvertPositionTo_Gcd_OfStepOffset(Vector3Int pos)
    {
        var newPosition = pos;
        newPosition.z -= pos.z % stepOffset;
        newPosition.x -= pos.x % stepOffset;
        return newPosition;
    }

    private void CreateRoomsFromPosition(HashSet<Vector3Int> roomPositions, List<Vector3Int> roomsToCreate)
    {
        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloors = CreateRoomFromPosition(roomPosition);
            roomPositions.UnionWith(roomFloors);
        }
    }

    protected HashSet<Vector3Int> CreateRoomFromPosition(Vector3Int startPosition)
    {
        var currentPosition = startPosition;

        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
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

    private List<Vector3Int> GetRoomNumberToSpawn(HashSet<Vector3Int> deadEndPositions, HashSet<Vector3Int> normalEndPositions)
    {
        // Shuffle
        int roomToCreateCount = Mathf.RoundToInt(normalEndPositions.Count * roomPercent);
        var potentialRoomList = normalEndPositions.ToList();
        potentialRoomList.Shuffle();
        List<Vector3Int> roomsToCreate = potentialRoomList.GetRange(0, roomToCreateCount);
        roomsToCreate.AddRange(deadEndPositions);
        return roomsToCreate;
    }


    private HashSet<Vector3Int> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        foreach (var room in roomList)
        {
            var roomBound = room;
            var roomCenter = new Vector3Int(Mathf.RoundToInt(roomBound.center.x), 0, Mathf.RoundToInt(roomBound.center.z));
            roomCenter = ConvertPositionTo_Gcd_OfStepOffset(roomCenter);
            var roomFloor = CreateRoomFromPosition(roomCenter);

            foreach (var roomPosition in roomFloor)
            {
                if (roomPosition.x >= (roomBound.xMin + offset) && roomPosition.x <= (roomBound.xMax - offset) &&
                roomPosition.z >= (roomBound.zMin + offset) && roomPosition.z <= (roomBound.zMax - offset))
                {
                    floorPositions.Add(roomPosition);
                }
            }
        }
        return floorPositions;
    }


    private HashSet<Vector3Int> CreateRoomsFromList(List<BoundsInt> roomList)
    {
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        foreach (var room in roomList)
        {
            for (int i = offset; i < room.size.x - offset; i += stepOffset)
            {
                for (int j = offset; j < room.size.z - offset; j += stepOffset)
                {
                    var floorPosition = room.min + new Vector3Int(i, 0, j);
                    // floorPosition = ConvertPositionTo_Gcd_OfStepOffset(floorPosition);
                    floorPositions.Add(floorPosition);
                }
            }
        }
        return floorPositions;
    }

    protected HashSet<Vector3Int> CreateCorridorsAndGetPotentialRoomPositions(HashSet<Vector3Int> deadEndRoomsPositions, HashSet<Vector3Int> normalRoomPositions)
    {
        HashSet<Vector3Int> corridorPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> allRoomsPositions = new HashSet<Vector3Int>();
        var currentPosition = startPosition;
        deadEndRoomsPositions.Add(currentPosition);
        for (int i = 0; i < corridorInteration; i++)
        {
            // get corridor positions
            var path = ProceduralGenrationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength, stepOffset);
            corridorPositions.UnionWith(path);
            currentPosition = path[path.Count - 1];
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

}

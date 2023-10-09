using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Generation;
using UnityEngine;

public class PartitionGeneration : MonoBehaviour
{
    int stepOffset;
    int roomOffset;

    public void Init(int stepOffset, int roomOffset)
    {
        this.stepOffset = stepOffset;
        this.roomOffset = roomOffset;
    }

    public Vector3Int ConvertPositionTo_Gcd_OfStepOffset(Vector3Int pos)
    {
        var newPosition = pos;
        newPosition.z -= pos.z % stepOffset;
        newPosition.x -= pos.x % stepOffset;
        return newPosition;
    }

    public List<BoundsInt> GetRoomByBinaryPartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight, int stepOffset)
    {
        Queue<BoundsInt> roomsQueue = new();
        List<BoundsInt> roomsList = new();
        roomsQueue.Enqueue(spaceToSplit);
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.x >= minWidth && room.size.z >= minHeight)
            {
                if (Random.value <= 0.5f)
                {
                    // split horizontally
                    if (room.size.z >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room, stepOffset);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room, stepOffset);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room, stepOffset);
                    }
                    else if (room.size.z >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room, stepOffset);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    private void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room, int stepOffset)
    {
        var zSplit = Random.Range((int)1 / stepOffset, (int)room.size.z / stepOffset) * stepOffset;
        var room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, room.size.y, zSplit));
        var room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y, room.min.z + zSplit), new Vector3Int(room.size.x, room.size.y, room.size.z - zSplit));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room, int stepOffset)
    {
        var xSplit = Random.Range((int)1 / stepOffset, (int)room.size.x / stepOffset) * stepOffset;
        var room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        var room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    public HashSet<Vector3Int> CreateCorridorBetweenTwoRoomsCenter(Vector3Int currentRoomCenter, Vector3Int closestRoomCenter)
    {
        HashSet<Vector3Int> corridor = new();
        var position = currentRoomCenter;
        position = ConvertPositionTo_Gcd_OfStepOffset(position);
        closestRoomCenter = ConvertPositionTo_Gcd_OfStepOffset(closestRoomCenter);

        while (position.z != closestRoomCenter.z)
        {
            if (position.z > closestRoomCenter.z)
                position += Vector3Int.back * stepOffset;
            else if (position.z < closestRoomCenter.z)
                position += Vector3Int.forward * stepOffset;
            corridor.Add(position);
        }


        while (position.x != closestRoomCenter.x)
        {
            if (position.x > closestRoomCenter.x)
                position += Vector3Int.left * stepOffset;
            else if (position.x < closestRoomCenter.x)
                position += Vector3Int.right * stepOffset;
            corridor.Add(position);
        }

        return corridor;
    }


    public HashSet<Vector3Int> CreateRoomsUseBP(List<BoundsInt> roomList)
    {
        HashSet<Vector3Int> floorPositions = new();
        foreach (var room in roomList)
        {
            for (int i = roomOffset; i < room.size.x - roomOffset; i += stepOffset)
            {
                for (int j = roomOffset; j < room.size.z - roomOffset; j += stepOffset)
                {
                    var floorPosition = room.min + new Vector3Int(i, 0, j);
                    floorPositions.Add(floorPosition);
                }
            }
        }
        return floorPositions;
    }


    public HashSet<Vector3Int> CreateRoomsUseRDW(List<BoundsInt> roomList, SimpleRandomWalkData simpleRandomWalkData)
    {
        HashSet<Vector3Int> floorPositions = new();
        foreach (var room in roomList)
        {
            var roomBound = room;
            var roomCenter = new Vector3Int(Mathf.RoundToInt(roomBound.center.x), 0, Mathf.RoundToInt(roomBound.center.z));
            roomCenter = ConvertPositionTo_Gcd_OfStepOffset(roomCenter);
            var roomFloor = CreateRoomFromPosition(roomCenter, simpleRandomWalkData);

            foreach (var roomPosition in roomFloor)
            {
                if (roomPosition.x >= (roomBound.xMin + roomOffset) && roomPosition.x <= (roomBound.xMax - roomOffset) &&
                roomPosition.z >= (roomBound.zMin + roomOffset) && roomPosition.z <= (roomBound.zMax - roomOffset))
                {
                    floorPositions.Add(roomPosition);
                }
            }
        }
        return floorPositions;
    }

    protected HashSet<Vector3Int> CreateRoomFromPosition(Vector3Int startPosition, SimpleRandomWalkData simpleRandomWalkData)
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


    public HashSet<Vector3Int> CreateCorridorsFromRoomList(List<BoundsInt> roomList)
    {
        List<Vector3Int> roomCenters = new();
        HashSet<Vector3Int> corridors = new();
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

    public Vector3Int FindClosetRoomCenter(Vector3Int currentRoomCenter, List<Vector3Int> roomCenters)
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
}

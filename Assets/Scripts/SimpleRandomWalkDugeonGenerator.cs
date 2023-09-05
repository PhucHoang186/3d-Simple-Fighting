using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using NaughtyAttributes;

public class SimpleRandomWalkDugeonGenerator : MonoBehaviour
{
    [SerializeField] int stepOffset;
    [SerializeField] int wallLayer;
    [Header("Room Generation")]
    [SerializeField] MapSpawner mapSpawner;
    [SerializeField] SimpleRandomWalkData simpleRandomWalkData;
    [SerializeField] protected Vector3Int startPosition = Vector3Int.zero;
    [SerializeField] private bool startRandomlyEachInteraction = true;
    [Header("Corridor Generation")]
    [SerializeField] int corridorLength;
    [SerializeField] int corridorInteration;
    [Range(0f, 1)]
    [SerializeField] float roomPercent;


    [Button]
    public void RunRoomGeneration()
    {
        HashSet<Vector3Int> floorPositions = CreateRoom(startPosition);
        mapSpawner.DestroyTiles();
        mapSpawner.SpawnFloorTiles(floorPositions, wallLayer);
        WallGenerator.CreateWalls(floorPositions, mapSpawner, stepOffset, wallLayer);
    }

    [Button]
    public void RunCorridorFirstGeneration()
    {
        HashSet<Vector3Int> floorPositions = CreateRoomsAndCorridors();
        mapSpawner.DestroyTiles();
        mapSpawner.SpawnFloorTiles(floorPositions, wallLayer);
        WallGenerator.CreateWalls(floorPositions, mapSpawner, stepOffset, wallLayer);

    }

    [Button]
    public void ClearMap()
    {
        mapSpawner.DestroyTiles();
    }

    private HashSet<Vector3Int> CreateRoomsAndCorridors()
    {
        HashSet<Vector3Int> roomPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> deadEndPositions = new HashSet<Vector3Int>();
        HashSet<Vector3Int> normalEndPositions = new HashSet<Vector3Int>();

        HashSet<Vector3Int> corridorPositions = CreateCorridorsAndGetPotentialRoomPositions(deadEndPositions, normalEndPositions);
        List<Vector3Int> roomsToCreate = GetRoomNumberToSpawn(deadEndPositions, normalEndPositions);

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloors = CreateRoom(roomPosition);
            roomPositions.UnionWith(roomFloors);
        }

        roomPositions.UnionWith(corridorPositions);

        return roomPositions;
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

    protected HashSet<Vector3Int> CreateRoom(Vector3Int startPosition)
    {
        var currentPosition = startPosition;
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        for (int i = 0; i < simpleRandomWalkData.interations; i++)
        {
            var path = ProceduralGenrationAlgorithms.SimpleRandomWalk(currentPosition, simpleRandomWalkData.walkLength, stepOffset);
            floorPositions.UnionWith(path);
            if (startRandomlyEachInteraction)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
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

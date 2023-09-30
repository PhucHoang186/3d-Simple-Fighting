using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generation
{
    public static class ProceduralGenrationAlgorithms
    {
        public static HashSet<Vector3Int> SimpleRandomWalk(Vector3Int startPosition, int walkLength, int stepOffset)
        {
            HashSet<Vector3Int> path = new();
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

        public static List<BoundsInt> BinaryPartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight, int stepOffset)
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

        private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room, int stepOffset)
        {
            var zSplit = Random.Range((int)1 / stepOffset, (int)room.size.z / stepOffset) * stepOffset;
            var room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, room.size.y, zSplit));
            var room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y, room.min.z + zSplit), new Vector3Int(room.size.x, room.size.y, room.size.z - zSplit));
            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }

        private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room, int stepOffset)
        {
            var xSplit = Random.Range((int)1 / stepOffset, (int)room.size.x / stepOffset) * stepOffset;
            var room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
            var room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }

        public static List<Vector3Int> RandomWalkCorridor(Vector3Int startPosition, int corridorLength, int stepOffset)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            path.Add(startPosition);
            var previousPosition = startPosition;
            var direction = Direction3D.GetRandomCardinalDirection(false);

            for (int i = 0; i < corridorLength; i++)
            {
                var newPosition = previousPosition + stepOffset * direction;
                path.Add(newPosition);
                previousPosition = newPosition;
            }
            return path;
        }

        public static RoomsData GetAllRoomsFloorDatas(HashSet<Vector3Int> floorPositions, int stepOffset)
        {
            RoomsData roomsDatas = new();
            roomsDatas.Init();
            var cardinalDirectionsList = Direction3D.GetCardinalDirectionsListIgnoreY();

            int neighborNumber = 4;
            foreach (var floorPosition in floorPositions)
            {
                // check 4 directions
                // top
                if (!floorPositions.Contains(floorPosition + stepOffset * cardinalDirectionsList[0]))
                {
                    roomsDatas.NearWallTopFloors.Add(floorPosition);
                    neighborNumber--;
                }
                // bottom
                if (!floorPositions.Contains(floorPosition + stepOffset * cardinalDirectionsList[1]))
                {
                    roomsDatas.NearWallDownFloors.Add(floorPosition);
                    neighborNumber--;
                }
                // right
                if (!floorPositions.Contains(floorPosition + stepOffset * cardinalDirectionsList[2]))
                {
                    roomsDatas.NearWallRightFloors.Add(floorPosition);
                    neighborNumber--;
                }
                // left
                if (!floorPositions.Contains(floorPosition + stepOffset * cardinalDirectionsList[3]))
                {
                    roomsDatas.NearWallLeftFloors.Add(floorPosition);
                    neighborNumber--;
                }
                // corner
                if (neighborNumber == 2)
                {
                    roomsDatas.NearCornerFloors.Add(floorPosition);
                }
                // non near wall
                if (neighborNumber == 4)
                {
                    roomsDatas.NonNearWallFloors.Add(floorPosition);
                }
            }

            return roomsDatas;
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
        Vector3Int.forward + Vector3Int.right, // top right
        Vector3Int.forward + Vector3Int.left, // top left
        Vector3Int.back + Vector3Int.right, // bottom right
        Vector3Int.back + Vector3Int.left, // bottom right
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

    public class RoomsData
    {
        public void Init()
        {
            NearWallTopFloors = new();
            NearWallDownFloors = new();
            NearWallRightFloors = new();
            NearWallLeftFloors = new();
            NonNearWallFloors = new();
            NearCornerFloors = new();
        }

        public HashSet<Vector3Int> NearWallTopFloors { get; set; }
        public HashSet<Vector3Int> NearWallDownFloors { get; set; }
        public HashSet<Vector3Int> NearWallRightFloors { get; set; }
        public HashSet<Vector3Int> NearWallLeftFloors { get; set; }
        public HashSet<Vector3Int> NonNearWallFloors { get; set; }
        public HashSet<Vector3Int> NearCornerFloors { get; set; }
    }
}
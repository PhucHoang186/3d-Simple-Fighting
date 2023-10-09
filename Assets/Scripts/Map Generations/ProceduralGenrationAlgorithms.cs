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

            foreach (var floorPosition in floorPositions)
            {
                // check 4 directions
                int neighborNumber = 4;
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

                roomsDatas.NearWallTopFloors.ExceptWith(roomsDatas.NearCornerFloors);
                roomsDatas.NearWallDownFloors.ExceptWith(roomsDatas.NearCornerFloors);
                roomsDatas.NearWallRightFloors.ExceptWith(roomsDatas.NearCornerFloors);
                roomsDatas.NearWallLeftFloors.ExceptWith(roomsDatas.NearCornerFloors);
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

        public static List<Vector3Int> eightDirectionsList = new List<Vector3Int>
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

        public static List<Vector3> eightNormalizedDirectionsList = new()
        {
            new Vector3(0, 0,1).normalized,
            new Vector3(0, 0,-1).normalized,
            new Vector3(1, 0, 0).normalized,
            new Vector3(-1, 0, 0).normalized,
            new Vector3(1, 0,1).normalized,
            new Vector3(-1, 0,1).normalized,
            new Vector3(1, 0,-1).normalized,
            new Vector3(-1, 0,-1).normalized,
        };

        public static Vector3Int GetRandomCardinalDirection(bool isThreeDemension = true)
        {
            var length = isThreeDemension ? eightDirectionsList.Count : eightDirectionsList.Count - 6;
            return eightDirectionsList[Random.Range(0, length)];
        }

        public static List<Vector3Int> GetCardinalDirectionsListIgnoreY()
        {
            return eightDirectionsList.GetRange(0, 4);
        }

        public static List<Vector3Int> GetDiagonalDirectionList()
        {
            return eightDirectionsList.GetRange(6, 4);
        }

        public static List<Vector3> GetNormalizeEightDirection()
        {
            List<Vector3> normalizedDirection = new();
            for (int i = 0; i < normalizedDirection.Count; i++)
            {
                normalizedDirection[i] = ((Vector3)eightDirectionsList[i]).normalized;
            }
            return normalizedDirection;
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
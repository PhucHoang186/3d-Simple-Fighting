using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using TMPro;

namespace Generation
{
    public class DugeonGenerator : MonoBehaviour
    {
        [SerializeField] int stepOffset;
        [SerializeField] int wallLayer;
        [SerializeField] MapSpawner mapSpawner;
        [SerializeField] PropsGenerator propsGenerator;
        [Header("Random Walk Procedural")]
        [SerializeField] RandomWalkGeneration randomWalkGeneration;
        [SerializeField] SimpleRandomWalkData randomWalkData;
        [SerializeField] Vector3Int startPosition;
        [SerializeField] int corridorLength;
        [SerializeField] int corridorInteration;
        [Range(0f, 1)]
        [SerializeField] float roomPercent;
        [SerializeField] int roomWidth;
        [SerializeField] int roomHeight;
        [Header("Binary Partition Procedural")]
        [SerializeField] PartitionGeneration partitionGeneration;
        [SerializeField] int dungeonWidth;
        [SerializeField] int dungeonHeight;
        [SerializeField] int minWidth;
        [SerializeField] int minHeight;
        [SerializeField] int roomOffset;
        [SerializeField] bool randomWalkRoom;
        [SerializeField] bool showRoomData;

        private HashSet<Vector3Int> floorPositions = new();
        private List<GameObject> allProps = new();
        private RoomsData roomsData;

        [Button]
        public void RunRandomWalkGeneration()
        {
            randomWalkGeneration.Init(startPosition, stepOffset, roomPercent, randomWalkRoom, randomWalkData, corridorLength, corridorInteration, roomWidth, roomHeight);
            floorPositions.Clear();
            HashSet<Vector3Int> corridorPositions;
            (floorPositions, corridorPositions) = randomWalkGeneration.CreateRoomsAndCorridors();
            roomsData = ProceduralGenrationAlgorithms.GetAllRoomsFloorDatas(floorPositions, stepOffset);
            floorPositions.UnionWith(corridorPositions);
            SpawnEnv(floorPositions);
        }

        [Button]
        public void RunBinaryPartitioningGeneration()
        {
            floorPositions.Clear();
            partitionGeneration.Init(stepOffset, roomOffset);
            BoundsInt areaBound = new(startPosition, new Vector3Int(dungeonWidth * stepOffset, 0, dungeonHeight * stepOffset));
            var roomList = partitionGeneration.GetRoomByBinaryPartitioning(areaBound, minWidth * stepOffset, minHeight * stepOffset, stepOffset);

            floorPositions = randomWalkRoom ? partitionGeneration.CreateRoomsUseRDW(roomList, randomWalkData) : partitionGeneration.CreateRoomsUseBP(roomList);
            HashSet<Vector3Int> corridorPositions = partitionGeneration.CreateCorridorsFromRoomList(roomList);

            roomsData = ProceduralGenrationAlgorithms.GetAllRoomsFloorDatas(floorPositions, stepOffset);
            floorPositions.UnionWith(corridorPositions);
            SpawnEnv(floorPositions);
        }

        [Button]
        public void SpawnProps()
        {
            HashSet<Vector3Int> availablePropPositions = new(floorPositions);
            allProps.AddRange(propsGenerator.GenerateProps(roomsData, availablePropPositions, stepOffset));
        }

        [Button]
        public void DeleteAllProps()
        {
            foreach (var prop in allProps)
            {
                DestroyImmediate(prop);
            }
            allProps.Clear();
        }

        [Button]
        public void ClearMap()
        {
            mapSpawner.DestroyTiles();
        }

        private void SpawnEnv(HashSet<Vector3Int> floorPositions)
        {
            mapSpawner.SpawnFloorTiles(floorPositions, wallLayer);
            mapSpawner.SpawnWalls(WallGenerator.CreateWalls(floorPositions, stepOffset), wallLayer);
            mapSpawner.SpawnCorners(WallGenerator.CreateCorners(floorPositions, stepOffset), wallLayer);
        }


        void OnDrawGizmos()
        {
            if (!showRoomData)
                return;
            DrawBox(roomsData.NearCornerFloors, Color.yellow);
            DrawBox(roomsData.NearWallTopFloors, Color.blue);
            DrawBox(roomsData.NearWallDownFloors, Color.blue);
            DrawBox(roomsData.NearWallLeftFloors, Color.blue);
            DrawBox(roomsData.NearWallRightFloors, Color.blue);
            DrawBox(roomsData.NonNearWallFloors, Color.cyan);
            DrawBox2(allProps, Color.red);
        }

        private void DrawBox(HashSet<Vector3Int> posiions, Color color)
        {
            Gizmos.color = color;
            foreach (var position in posiions)
            {
                var newPosition = (Vector3)position;
                newPosition.y -= 0.5f;
                Gizmos.DrawCube(newPosition, Vector3.one * 4);
            }
        }
        private void DrawBox2(List<GameObject> posiions, Color color)
        {
            Gizmos.color = color;
            foreach (var position in posiions)
            {
                var newPosition = position.transform.position;
                Gizmos.DrawCube(newPosition, Vector3.one * 4);
            }
        }
    }
}
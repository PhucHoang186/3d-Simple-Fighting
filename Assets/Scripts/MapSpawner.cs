using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] MapSpawnData mapdata;
    [SerializeField] List<GameObject> wallTilePrefabs;
    [SerializeField] List<GameObject> cornerTilePrefabs;
    [SerializeField] int wallHeight;
    private List<GameObject> tiles = new List<GameObject>();

    public void SpawnFloorTiles(IEnumerable<Vector3Int> floorPositions, int wallLayer = 1)
    {
        foreach (var floorPosition in floorPositions)
        {
            SpawnTile(mapdata.GetFloorPrefab(), floorPosition, Quaternion.identity, this.transform);
            var ceilingPosition = floorPosition;
            // ceilingPosition.y += wallHeight * wallLayer;
            // SpawnTile(mapdata.GetCeilingPrefab(), ceilingPosition, quaternion.identity, this.transform);
        }
    }

    public void SpawnTile(GameObject tilePrefab, Vector3 tilePosition, Quaternion rotation, Transform parent = null)
    {
        var newTile = Instantiate(tilePrefab, tilePosition, quaternion.identity);
        newTile.transform.parent = parent;
        tiles.Add(newTile);
    }

    public void SpawnWalls(IEnumerable<WallData> wallDatas, int wallLayer = 1)
    {
        foreach (var wallData in wallDatas)
        {
            for (int i = 0; i < wallLayer; i++)
            {
                var wallPrefab = GetWallPrefab(wallData.wallType);
                var wallPosition = wallData.wallPosition;
                wallPosition.y += wallHeight * i;
                SpawnTile(wallPrefab, wallPosition, Quaternion.identity, this.transform);
            }
        }
    }

    public void SpawnCorners(IEnumerable<CornerData> cornerDatas, int wallLayer = 1)
    {
        foreach (var cornerData in cornerDatas)
        {
            for (int i = 0; i < wallLayer; i++)
            {
                var cornerPrefab = GetCornerPrefab(cornerData.cornerType);
                var cornerPosition = cornerData.cornerPosition;
                cornerPosition.y += wallHeight * i;
                SpawnTile(cornerPrefab, cornerPosition, Quaternion.identity, this.transform);
            }
        }
    }

    private GameObject GetWallPrefab(WallType wallType)
    {
        var wallPrefab = wallTilePrefabs[(int)wallType];
        return wallPrefab;
    }

    private GameObject GetCornerPrefab(CornerType cornerType)
    {
        var cornerPrefab = cornerTilePrefabs[(int)cornerType];
        return cornerPrefab;
    }

    public void DestroyTiles()
    {
        foreach (var tile in tiles)
        {
            DestroyImmediate(tile.gameObject);
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        tiles.Clear();
    }
}

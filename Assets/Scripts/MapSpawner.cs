using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] MapSpawnData mapdata;

    [SerializeField] int wallHeight;
    [SerializeField] bool spawnCeiling;
    private List<GameObject> tiles = new List<GameObject>();

    public void SpawnFloorTiles(IEnumerable<Vector3Int> floorPositions, int wallLayer = 1)
    {
        foreach (var floorPosition in floorPositions)
        {
            SpawnTile(mapdata.GetRandomFloorPrefab(), floorPosition, Quaternion.identity, this.transform);
            var ceilingPosition = floorPosition;
            if (spawnCeiling)
            {
                ceilingPosition.y += wallHeight * wallLayer;
                SpawnTile(mapdata.GetRandomCeilingPrefab(), ceilingPosition, quaternion.identity, this.transform);
            }
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
        var wallPrefab = mapdata.wallTilePrefabs[(int)wallType];
        return wallPrefab;
    }

    private GameObject GetCornerPrefab(CornerType cornerType)
    {
        var cornerPrefab = mapdata.cornerTilePrefabs[(int)cornerType];
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

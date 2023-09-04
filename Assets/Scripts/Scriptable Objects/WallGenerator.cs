using System;
using System.Collections.Generic;
using UnityEngine;

public enum WallType
{
    WallTop,
    WallBottom,
    WallRight,
    WallLeft,

}
public enum CornerType
{
    CornerTopRight,
    CornerTopLeft,
    CornerBottomRight,
    CornerBottomLeft,
}

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector3Int> floorPositions, MapSpawner mapSpawner, int stepOffset, int wallLayer = 1)
    {
        var wallDatas = FindWallInDirections(floorPositions, Direction3D.GetCardinalDirectionsListIgnoreY(), stepOffset);
        mapSpawner.SpawnWalls(wallDatas, wallLayer);
        var cornerDatas = FindCornerInDirections(floorPositions, Direction3D.GetExtraDirectionList(), stepOffset);
        mapSpawner.SpawnCorners(cornerDatas, wallLayer);
    }

    private static HashSet<WallData> FindWallInDirections(HashSet<Vector3Int> floorPositions, List<Vector3Int> directionsList, int stepOffset)
    {
        HashSet<WallData> wallDatas = new HashSet<WallData>();
        foreach (var position in floorPositions)
        {
            for (int i = 0; i < directionsList.Count; i++)
            {
                var neighbourPosition = position + directionsList[i] * stepOffset;
                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    var newWallData = new WallData();
                    newWallData.wallType = (WallType)i;
                    newWallData.wallPosition = neighbourPosition;
                    wallDatas.Add(newWallData);
                }
            }
        }
        return wallDatas;
    }

    private static HashSet<CornerData> FindCornerInDirections(HashSet<Vector3Int> floorPositions, List<Vector3Int> directionsList, int stepOffset)
    {
        HashSet<CornerData> cornerDatas = new HashSet<CornerData>();
        HashSet<Vector3Int> cornerPositions = new HashSet<Vector3Int>();
        foreach (var position in floorPositions)
        {
            for (int i = 0; i < directionsList.Count; i++)
            {
                var neighbourPosition = position + directionsList[i] * stepOffset;
                if (floorPositions.Contains(neighbourPosition) == false)
                //  && cornerPositions.Contains(neighbourPosition) == false)
                {
                    var newCornerData = new CornerData();
                    newCornerData.cornerType = (CornerType)i;
                    newCornerData.cornerPosition = neighbourPosition;
                    cornerPositions.Add(neighbourPosition);
                    cornerDatas.Add(newCornerData);
                }
            }
        }
        return cornerDatas;
    }
}

[Serializable]
public class WallData
{
    public Vector3Int wallPosition;
    public WallType wallType;
}

[Serializable]
public class CornerData
{
    public Vector3Int cornerPosition;
    public CornerType cornerType;
}

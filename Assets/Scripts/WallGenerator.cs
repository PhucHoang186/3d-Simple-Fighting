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
        var wallDatas = FindWallInDirections(floorPositions, stepOffset);
        mapSpawner.SpawnWalls(wallDatas, wallLayer);
        var cornerDatas = FindCornerInDirections(floorPositions, stepOffset);
        mapSpawner.SpawnCorners(cornerDatas, wallLayer);
    }

    private static HashSet<WallData> FindWallInDirections(HashSet<Vector3Int> floorPositions, int stepOffset)
    {
        HashSet<WallData> wallDatas = new HashSet<WallData>();
        List<Vector3Int> directionsList = Direction3D.GetCardinalDirectionsListIgnoreY();
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

    private static HashSet<CornerData> FindCornerInDirections(HashSet<Vector3Int> floorPositions, int stepOffset)
    {
        HashSet<CornerData> cornerDatas = new HashSet<CornerData>();
        var extraDirectionList = Direction3D.GetExtraDirectionList();
        var cardinalDirectionList = Direction3D.GetCardinalDirectionsListIgnoreY();
        foreach (var position in floorPositions)
        {
            for (int i = 0; i < extraDirectionList.Count; i++)
            {
                var cornerPosition = position + extraDirectionList[i] * stepOffset;
                if (floorPositions.Contains(cornerPosition) == false)
                {
                    var newCornerData = new CornerData();
                    newCornerData.cornerType = (CornerType)i;
                    newCornerData.cornerPosition = cornerPosition;
                    // var floorNeighborCount = 0;
                    // var checkFloorPosition = Vector3Int.zero;
                    // for (int j = 0; j < cardinalDirectionList.Count; j++)
                    // {
                    //     checkFloorPosition = cornerPosition + cardinalDirectionList[j] * stepOffset;
                    //     if (floorPositions.Contains(checkFloorPosition))
                    //     {
                    //         floorNeighborCount += 1;
                    //         Debug.Log(checkFloorPosition + " " + floorNeighborCount + " " + cornerPosition);
                    //     }
                    // }

                    // if (floorNeighborCount != 1)
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

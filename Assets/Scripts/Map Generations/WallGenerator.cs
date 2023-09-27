using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generation
{
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
        public static HashSet<WallData> CreateWalls(HashSet<Vector3Int> floorPositions, int stepOffset)
        {
            return FindWallInDirections(floorPositions, stepOffset);
        }


        public static HashSet<CornerData> CreateCorners(HashSet<Vector3Int> floorPositions, int stepOffset)
        {
            return FindCornerInDirections(floorPositions, stepOffset);
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
}
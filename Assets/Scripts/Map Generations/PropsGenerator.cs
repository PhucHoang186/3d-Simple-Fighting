using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generation
{
    public class PropsGenerator : MonoBehaviour
    {
        [SerializeField] List<PropData> propDatas;
        [SerializeField, Range(0, 1)]
        float cornerPropPlacementRate = 0.7f;
        [SerializeField, Range(0, 1)]
        float wallPropPlacementRate = 0.7f;
        private List<GameObject> propsSpawnedList = new();
        private int stepOffset;

        public List<GameObject> GenerateProps(RoomsData roomsData, HashSet<Vector3Int> availablePositions, int stepOffset)
        {
            this.stepOffset = stepOffset;
            List<GameObject> props = new();
            // corner
            List<PropData> nearCornerProps = propDatas.Where(x => x.Corner).ToList();
            props.AddRange(PlaceProps(roomsData.NearCornerFloors, nearCornerProps, availablePositions, cornerPropPlacementRate));
            // top
            List<PropData> nearTopProps = propDatas.Where(x => x.NearWallTop).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NearWallTopFloors, nearTopProps, availablePositions, wallPropPlacementRate));
            // bottom
            List<PropData> nearBottomProps = propDatas.Where(x => x.NearWallBottom).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NearWallDownFloors, nearBottomProps, availablePositions, wallPropPlacementRate));
            // left
            List<PropData> nearLeftProps = propDatas.Where(x => x.NearWallLeft).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NearWallLeftFloors, nearLeftProps, availablePositions, wallPropPlacementRate));
            // right
            List<PropData> nearRightProps = propDatas.Where(x => x.NearWallRight).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NearWallRightFloors, nearRightProps, availablePositions, wallPropPlacementRate));
            // center
            List<PropData> centerProps = propDatas.Where(x => x.Inner).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NonNearWallFloors, centerProps, availablePositions, wallPropPlacementRate));
            return props;
        }

        private GameObject PlacePropAtPostition(Vector3Int placePostition, PropData propData, HashSet<Vector3Int> availablePositions)
        {

            if (Random.value > propData.SpawnPercent)
                return null;

            if (!CheckPlaceable(availablePositions, placePostition, propData.PropSize.x, propData.PropSize.y))
                return null;
            var propSpawned = propData.SpawnProp(placePostition, this.transform);
            propsSpawnedList.Add(propSpawned);
            availablePositions.Remove(placePostition);
            return propSpawned;
        }

        private List<GameObject> PlaceProps(HashSet<Vector3Int> floorPositions, List<PropData> propDatas, HashSet<Vector3Int> availablePositions, float spawnPercent)
        {
            List<GameObject> props = new();
            foreach (var propData in propDatas)
            {
                foreach (var floorPosition in floorPositions)
                {
                    if (propData.PlaceAsGroup)
                    {
                        props.AddRange(PlaceGroupPropObjects(floorPosition, propData, availablePositions));
                    }
                    else
                    {
                        var prop = PlacePropAtPostition(floorPosition, propData, availablePositions);
                        if (prop != null)
                            props.Add(prop);
                    }
                }
            }
            return props;
        }

        private List<GameObject> PlaceGroupPropObjects(Vector3Int placePostition, PropData propData, HashSet<Vector3Int> availablePositions)
        {
            var searchOffset = 1; // 8 direct
            int spawnNumber = Random.Range(propData.GroupMinCount, propData.GroupMaxCount + 1);
            List<GameObject> props = new();

            List<Vector3Int> propGroupPositions = new() { placePostition };
            for (int x = -searchOffset; x <= searchOffset; x++)
            {
                for (int z = -searchOffset; z <= searchOffset; z++)
                {
                    var checkPosition = placePostition + new Vector3Int(x, 0, z) * stepOffset;
                    if (availablePositions.Contains(checkPosition) && (x != 0 || z != 0))
                    {
                        propGroupPositions.Add(checkPosition);
                    }
                }
            }

            int spawnCount = Mathf.Min(spawnNumber, propGroupPositions.Count);
            for (int i = 0; i < spawnCount; i++)
            {
                var prop = PlacePropAtPostition(propGroupPositions[i], propData, availablePositions);
                if (prop != null)
                    props.Add(prop);
            }
            return props;
        }

        private bool CheckPlaceable(HashSet<Vector3Int> availablePostitions, Vector3Int placePosition, int width, int height)
        {
            HashSet<Vector3Int> propPosition = new();
            for (int i = 0; i < width; i++)
            {
                var position = placePosition + Vector3Int.right * i * stepOffset;
                propPosition.Add(position);
                if (!availablePostitions.Contains(position))
                {
                    Debug.Log("can't place on X");
                    return false;
                }
            }

            for (int i = 0; i < height; i++)
            {
                var position = placePosition + Vector3Int.forward * i * stepOffset;
                propPosition.Add(position);
                if (!availablePostitions.Contains(position))
                {
                    Debug.Log("can't place on Y");
                    return false;
                }
            }
            availablePostitions.ExceptWith(propPosition);
            return true;
        }
    }
}

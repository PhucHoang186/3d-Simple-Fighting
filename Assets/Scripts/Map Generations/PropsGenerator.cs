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

        public List<GameObject> GenerateProps(RoomsData roomsData, HashSet<Vector3Int> availablePositions, int stepOffset)
        {
            List<GameObject> props = new();
            // corner
            List<PropData> nearCornerProps = propDatas.Where(x => x.Corner).ToList();
            props.AddRange(PlaceProps(roomsData.NearCornerFloors, nearCornerProps, availablePositions, cornerPropPlacementRate));
            // top
            List<PropData> nearTopProps = propDatas.Where(x => x.NearWallTop).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NearWallTopFloors, nearTopProps, availablePositions, wallPropPlacementRate));
            // bottom
            List<PropData> nearBottomProps = propDatas.Where(x => x.NearWallTop).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NearWallTopFloors, nearBottomProps, availablePositions, wallPropPlacementRate));
            // left
            List<PropData> nearLeftProps = propDatas.Where(x => x.NearWallTop).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NearWallTopFloors, nearLeftProps, availablePositions, wallPropPlacementRate));
            // right
            List<PropData> nearRightProps = propDatas.Where(x => x.NearWallTop).OrderByDescending(x => x.PropPerimeter).ToList();
            props.AddRange(PlaceProps(roomsData.NearWallTopFloors, nearRightProps, availablePositions, wallPropPlacementRate));
            return props;
        }

        private GameObject PlacePropAtPostition(Vector3Int placePostition, List<PropData> listProps, HashSet<Vector3Int> availablePositions)
        {
            var propSpawned = listProps.PickRandomValueFromList().SpawnProp(placePostition, this.transform);
            propsSpawnedList.Add(propSpawned);
            availablePositions.Remove(placePostition);
            return propSpawned;
        }

        // private List<GameObject> PlacePropsInCorner(HashSet<Vector3Int> cornerPositions, List<PropData> cornerProps, HashSet<Vector3Int> availablePositions, float spawnPercent)
        // {
        //     List<GameObject> props = new();
        //     foreach (var cornerPosition in cornerPositions)
        //     {
        //         if (Random.value <= cornerPropPlacementRate)
        //         {
        //             props.Add(PlacePropAtPostition(cornerPosition, cornerProps, availablePositions, ));
        //         }
        //     }
        //     return props;
        // }

        private List<GameObject> PlaceProps(HashSet<Vector3Int> floorPositions, List<PropData> cornerProps, HashSet<Vector3Int> availablePositions, float spawnPercent)
        {
            List<GameObject> props = new();
            foreach (var floorPosition in floorPositions)
            {
                if (Random.value <= spawnPercent)
                {
                    props.Add(PlacePropAtPostition(floorPosition, cornerProps, availablePositions));
                }
            }
            return props;
        }
    }
}

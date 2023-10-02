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
        private List<GameObject> propsSpawnedList = new();

        public void GenerateProps(RoomsData roomsData, HashSet<Vector3Int> availablePositions, int stepOffset)
        {
            List<PropData> nearCornerProps = propDatas.Where(x => x.Corner).ToList();
            PlacePropsInCorner(roomsData.NearCornerFloors, nearCornerProps, availablePositions);

        }

        private void PlacePropAtPostition(Vector3Int placePostition, List<PropData> listProps, HashSet<Vector3Int> availablePositions)
        {
            var cornerPropSpawned = listProps.PickRandomValueFromList().SpawnProp(placePostition, this.transform);
            propsSpawnedList.Add(cornerPropSpawned);
            availablePositions.Remove(placePostition);
        }

        private void PlacePropsInCorner(HashSet<Vector3Int> cornerPositions, List<PropData> cornerProps, HashSet<Vector3Int> availablePositions)
        {
            foreach (var cornerPosition in cornerPositions)
            {
                if (Random.value <= cornerPropPlacementRate)
                {
                    PlacePropAtPostition(cornerPosition, cornerProps, availablePositions);
                }
            }
        }
    }
}

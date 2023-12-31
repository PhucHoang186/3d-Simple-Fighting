using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generation
{
    [CreateAssetMenu]
    public class PropData : ScriptableObject
    {
        [Header("Prop Data:")]
        public Vector2Int PropSize;
        public GameObject PropPrefab;
        [Range(0, 1)]
        public float SpawnPercent = 0.3f;
        [Header("Placement Type:")]
        public bool Corner = true;
        public bool NearWallTop = true;
        public bool NearWallRight = true;
        public bool NearWallBottom = true;
        public bool NearWallLeft = true;
        public bool Inner = true;
        [Min(1)]
        public int PlacementQuanityMin = 1;
        [Min(1)]
        public int PlacementQuanityMax = 1;

        [Space, Header("Group Placement:")]
        public bool PlaceAsGroup;
        [Min(1)]
        public int GroupMinCount = 1;
        [Min(1)]
        public int GroupMaxCount = 1;
        public int PropPerimeter => PropSize.x * PropSize.y;

        public GameObject SpawnProp(Vector3 floorPosition, Transform parent)
        {
            var propObj = Instantiate(PropPrefab, parent);
            propObj.transform.ResetTransform();
            propObj.transform.position = floorPosition;
            return propObj;
        }
    }
}

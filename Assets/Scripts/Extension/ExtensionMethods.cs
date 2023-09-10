using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class ExtensionMethods
{
    public static void Shuffle<T>(this IList<T> list)
    {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    public static void ResetTransform(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }
}

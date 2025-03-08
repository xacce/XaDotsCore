using System.Diagnostics;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Core.Hybrid.UtilityBakers
{
    public static class ArrayBakingExtension
    {
        [Conditional("UNITY_EDITOR")]
        public static void BakeFromArray(this DynamicBuffer<Entity> buffer, IBaker baker, GameObject[] objects, TransformUsageFlags usageFlags = TransformUsageFlags.Dynamic)
        {
            buffer.CopyFrom(objects.ToList().ConvertAll(input => baker.GetEntity(input, usageFlags)).ToArray());
        }

        [Conditional("UNITY_EDITOR")]
        public static void BakeFromArray<T>(this DynamicBuffer<Entity> buffer, IBaker baker, T[] objects, TransformUsageFlags usageFlags = TransformUsageFlags.Dynamic)
            where T : MonoBehaviour
        {
            buffer.CopyFrom(objects.ToList().ConvertAll(input => baker.GetEntity(input, usageFlags)).ToArray());
        }
    }
}
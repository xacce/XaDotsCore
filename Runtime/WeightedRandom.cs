using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Properties;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DotsCore.Utils
{
    [InternalBufferCapacity(0)]
    public partial struct WeightedEntity : IBufferElementData, IWeighted
    {
#if UNITY_EDITOR
        [CreateProperty]
#endif
        public float weight { get; set; }

        public Entity entity;
    }

    [Serializable]
    public class WeightedEntityBaked
    {
        [SerializeField] float weight;
        [SerializeField] GameObject go;

        public WeightedEntity Bake(IBaker baker) => new WeightedEntity()
        {
            weight = weight,
            entity = baker.GetEntity(go, TransformUsageFlags.None),
        };
    }

    [Serializable]
    public class WeightedEntitiesBaked
    {
        [SerializeField] WeightedEntityBaked[] entities = Array.Empty<WeightedEntityBaked>();

        public void ToBuffer(IBaker baker, Entity to)
        {
            baker.AddBuffer<WeightedEntity>(to).CopyFrom(entities.ToList().ConvertAll(input => input.Bake(baker)).ToArray());
        }
    }

    public interface IWeighted
    {
        public float weight { get; }
    }

    public interface IWeightedCollector<T> where T : unmanaged, IWeighted
    {
        public bool IsValid(T data);
    }

    [BurstCompile]
    public static class EntityRandomWeight
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetRandomFromFloats(in NativeArray<float3> items, ref Random rng, out float3 outT)
        {
            var weightSum = 0f;
            foreach (var item in items)
            {
                weightSum += item.z;
            }

            var value = rng.NextFloat(0, weightSum);
            var current = 0f;

            foreach (var item in items)
            {
                current += item.z;

                if (current > value)
                {
                    outT = item;
                    return true;
                }
            }

            outT = default;
            return false;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetRandomWeighted(in NativeHashMap<int, float> items, ref Random rng, out int outT)
        {
            var weightSum = 0f;
            foreach (var kvPair in items)
            {
                weightSum += kvPair.Value;
            }

            var value = rng.NextFloat(0, weightSum);
            var current = 0f;

            foreach (var kvPair in items)
            {
                current += kvPair.Value;

                if (current > value)
                {
                    outT = kvPair.Key;
                    return true;
                }
            }

            outT = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetRandomWeighted<T>(ref BlobArray<T> items, in float weightSum, ref Random rng, out int index) where T : unmanaged, IWeighted
        {
            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetRandomWeighted(ref BlobArray<float> items, in float weightSum, ref Random rng, out int index)
        {
            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i];
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetRandomWeighted<T>(in INativeList<T> items, in float weightSum, ref Random rng, out int index) where T : unmanaged, IWeighted
        {
            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetRandomWeighted<T>(in DynamicBuffer<T> items, in float weightSum, ref Random rng, out int index) where T : unmanaged, IWeighted
        {
            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetRandomWeighted<T, TC>(in NativeArray<T> items, ref Random rng, TC collector, out int index)
            where T : unmanaged, IWeighted where TC : unmanaged, IWeightedCollector<T>
        {
            var weightSum = 0f;

            for (int i = 0; i < items.Length; i++)
            {
                if (collector.IsValid(items[i])) weightSum += items[i].weight;
            }

            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                if (!collector.IsValid(items[i])) continue;
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }
#if LATIOS
        [BurstCompile]
        public static bool GetRandomWeightedCustom<T>(in NativeHashMap<T, float> items, ref SystemRng rng, out T outT) where T : unmanaged, IEquatable<T> //latios
        {
            var weightSum = 0f;
            foreach (var kvPair in items)
            {
                weightSum += kvPair.Value;
            }

            var value = rng.NextFloat(0, weightSum);
            var current = 0f;

            foreach (var kvPair in items)
            {
                current += kvPair.Value;

                if (current > value)
                {
                    outT = kvPair.Key;
                    return true;
                }
            }

            outT = default;
            return false;
        }

        [BurstCompile]
        public static bool GetRandomWeighted<T>(ref BlobArray<T> items, in float weightSum, ref SystemRng rng, out int index) where T : unmanaged, IWeighted
        {
            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }
        [BurstCompile]
        public static bool GetRandomWeighted<T>(ref NativeList<T> items, in float weightSum, ref SystemRng rng, out int index) where T : unmanaged, IWeighted
        {
            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }
 [BurstCompile]
        public static bool GetRandomIndex<T>(in DynamicBuffer<T> items, ref SystemRng rng, out int index) where T : unmanaged, IWeighted
        {
            var weightSum = 0f;
            foreach (var v in items)
            {
                weightSum += v.weight;
            }
            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }
#endif

        [BurstCompile]
        public static bool GetRandomIndexFromNativeArray(ref NativeArray<float> items, ref Random rng, out int index)
        {
            var weightSum = 0f;
            foreach (var v in items)
            {
                weightSum += v;
            }

            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i];
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }

        [BurstCompile]
        public static bool GetRandomWeighted<T>(in NativeArray<T> items, ref Random rng, out int index) where T : unmanaged, IWeighted
        {
            var weightSum = 0f;
            foreach (var v in items)
            {
                weightSum += v.weight;
            }

            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }

        [BurstCompile]
        public static bool GetRandomWeighted<T>(in NativeList<T> items, ref Random rng, out int index) where T : unmanaged, IWeighted
        {
            var weightSum = 0f;
            foreach (var v in items)
            {
                weightSum += v.weight;
            }

            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }

        [BurstCompile]
        public static bool GetRandomIndex<T>(in DynamicBuffer<T> items, ref Random rng, out int index) where T : unmanaged, IWeighted
        {
            var weightSum = 0f;
            foreach (var v in items)
            {
                weightSum += v.weight;
            }

            var value = rng.NextFloat(0, weightSum);
            var current = 0f;
            for (int i = 0; i < items.Length; i++)
            {
                current += items[i].weight;
                if (current > value)
                {
                    index = i;
                    return true;
                }
            }

            index = 0;
            return false;
        }
    }
}
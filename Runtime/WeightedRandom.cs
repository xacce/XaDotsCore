using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace DotsCore.Utils
{
    public interface IWeighted
    {
        public float weight { get; }
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
        public static bool GetRandomWeighted<T>(in NativeList<T> items, in float weightSum, ref Random rng, out int index) where T : unmanaged, IWeighted
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
        public static bool GetRandomIndexFromNativeArrayCustom<T>(in NativeArray<T> items, ref Random rng, out int index) where T : unmanaged, IWeighted
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
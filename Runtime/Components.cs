﻿using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
#if LOCALIZATION_PACKAGE_EXISTS
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
#endif

namespace Core.Runtime
{
#if LOCALIZATION_PACKAGE_EXISTS
    public partial struct LocalizedStringTableReferenceBaked
    {
        public Guid table;
        public long key;

        public static implicit operator LocalizedStringTableReferenceBaked(LocalizedString str)
        {
            return new LocalizedStringTableReferenceBaked()
            {
                table = str.TableReference.TableCollectionNameGuid,
                key = str.TableEntryReference.KeyId,
            };
        }

        public static explicit operator TableReference(LocalizedStringTableReferenceBaked str)
        {
            return str.table;
        }

        public static explicit operator TableEntryReference(LocalizedStringTableReferenceBaked str)
        {
            return str.key;
        }
    }

#endif
    public partial struct NotUsedFreeToRemove : IComponentData
    {
    }


    public partial struct Bump : IComponentData
    {
    }

    public partial struct Triggered : IComponentData
    {
    }

    public partial struct TriggeredBy : IComponentData
    {
        public Entity actor;
    }

    public partial struct RootParent : IComponentData
    {
        public Entity value;
    }

    public partial struct AutoDiscoverRootParentTag : IComponentData
    {
    }

    public partial struct DynamicObjectVelocity : IComponentData
    {
        public float3 velocity;
    }

    [BurstCompile]
    public struct AnimationCurveBlob
    {
        public BlobArray<float> samples;
        public int length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public float GetValueAtTime(float time)
        {
            if (time < 0) time = 0;
            var approxSampleIndex = (length - 1) * time;
            var sampleIndexBelow = (int)math.floor(approxSampleIndex);
            if (sampleIndexBelow >= length - 1) return samples[length - 1];
            var indexRemainder = approxSampleIndex - sampleIndexBelow;
            return math.lerp(samples[sampleIndexBelow], samples[sampleIndexBelow + 1], indexRemainder);
        }

        public bool IsValid() => length > 0;
        public static AnimationCurveBlob Null => new AnimationCurveBlob() { length = 0 };
    }

    [BurstCompile]
    public struct AnimationCurveFloat3Blob
    {
        public BlobArray<float3> samples;
        public int length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public float3 GetValueAtTime(float time)
        {
            if (time < 0) time = 0;
            var approxSampleIndex = (length - 1) * time;
            var sampleIndexBelow = (int)math.floor(approxSampleIndex);
            if (sampleIndexBelow >= length - 1) return samples[length - 1];
            var indexRemainder = approxSampleIndex - sampleIndexBelow;
            return math.lerp(samples[sampleIndexBelow], samples[sampleIndexBelow + 1], indexRemainder);
        }

         public bool IsValid() => length > 0;
               public static AnimationCurveFloat3Blob Null => new AnimationCurveFloat3Blob() { length = 0 };
    }

    public partial struct GameReadySingleton : IComponentData
    {
    }
}
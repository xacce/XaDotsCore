using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Core.Runtime
{
    public partial struct Triggerable : IComponentData
    {
    }

    public partial struct Triggered : IComponentData
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
    }
}
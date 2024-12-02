using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Core.Runtime.PerlinNoise
{
    public partial struct PerlinNoiseWorldSpaceCombinedBlob
    {
        public BlobArray<PerlinNoise3dParams> positions;
        public BlobArray<PerlinNoise3dParams> directions;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetCombined(ref BlobArray<PerlinNoise3dParams> prms, float time, float3 timeOffsets)
        {
            float3 pos = new float3();
            for (int i = 0; i < prms.Length; ++i)
                pos += PerlinNoise3dParams.GetValueAt(prms[i], time, timeOffsets);

            return pos;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetCombined( PerlinNoise3dParams[] prms, float time, float3 timeOffsets)
        {
            float3 pos = new float3();
            for (int i = 0; i < prms.Length; ++i)
                pos += PerlinNoise3dParams.GetValueAt(prms[i], time, timeOffsets);

            return pos;
        }
    }
    [Serializable]

    public partial struct PerlinNoise3dParams
    {
        public PerlinNoiseParams x;
        public PerlinNoiseParams y;
        public PerlinNoiseParams z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetValueAt(in PerlinNoise3dParams prms, float time, float3 timeOffsets)
        {
            return new float3(
                PerlinNoiseParams.GetValueAt(prms.x, time, timeOffsets.x),
                PerlinNoiseParams.GetValueAt(prms.y, time, timeOffsets.y),
                PerlinNoiseParams.GetValueAt(prms.z, time, timeOffsets.z));
        }
    }
    [Serializable]
    public partial struct PerlinNoiseParams
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetValueAt(in PerlinNoiseParams prms, float time, float timeOffset)
        {
            float t = (prms.frequency * time) + timeOffset;
            if (prms.constant)
                return math.cos(t * 2 * math.PI) * prms.amplitude * 0.5f;
            return noise.cnoise(new float2(t, 0f) - 0.5f) * prms.amplitude;
        }
#if UNITY_EDITOR
        [Tooltip("The frequency of noise for this channel.  Higher magnitudes vibrate faster.")]
#endif
        public float frequency;

        /// <summary>The amplitude of the noise for this channel.  Larger numbers vibrate higher</summary>
#if UNITY_EDITOR
        [Tooltip("The amplitude of the noise for this channel.  Larger numbers vibrate higher.")]
#endif
        public float amplitude;

        /// <summary>If checked, then the amplitude and frequency will not be randomized</summary>

#if UNITY_EDITOR
        [Tooltip("If checked, then the amplitude and frequency will not be randomized.")]
#endif
        public bool constant;
    }
}
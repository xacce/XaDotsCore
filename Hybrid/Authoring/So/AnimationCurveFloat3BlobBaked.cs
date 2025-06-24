#if UNITY_EDITOR
using System;
using Core.Hybrid;
using Core.Runtime;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Core.Hybrid
{
    [CreateAssetMenu(menuName = "XaDotsCore/Animation curve float 3")]
    public class AnimationCurveFloat3BlobBaked : BakedScriptableObject<AnimationCurveFloat3Blob>
    {
        [SerializeField] private AnimationCurve x;
        [SerializeField] private AnimationCurve y;
        [SerializeField] private AnimationCurve z;
        [SerializeField] private int precision_s = 64;
        [SerializeField] private float globalMultiplier = 1;


        public int precision => precision_s;

        public override void Bake(ref AnimationCurveFloat3Blob data, ref BlobBuilder builder)
        {
            var array = builder.Allocate(ref data.samples, precision_s);
            data.length = precision_s;
            for (var i = 0; i < data.length; i++)
            {
                var at = (float)i / (precision_s - 1);
                var value = new float3(x.Evaluate(at), y.Evaluate(at), z.Evaluate(at)) * globalMultiplier;
                array[i] = value;
            }
        }
    }
}
#endif
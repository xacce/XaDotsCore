#if UNITY_EDITOR
using Unity.Entities;
using UnityEngine;
using XaDotsCore.Editor.So;

namespace XaDotsCore.Runtime.Authoring.So
{
    [CreateAssetMenu(menuName = "XaDotsCore/Animation curve")]
    public class AnimationCurveBlobBaked : BakedScriptableObject<AnimationCurveBlob>
    {
        [SerializeField] private AnimationCurve curve_s;
        [SerializeField] private int precision_s = 64;

        public override void Bake(ref AnimationCurveBlob data, ref BlobBuilder builder)
        {
            var array = builder.Allocate(ref data.samples, precision_s);
            data.length = precision_s;
            for (var i = 0; i < precision_s; i++)
            {
                var at = (float)i / (precision_s - 1);
                var value = curve_s.Evaluate(at);
                array[i] = value;
            }
        }
    }
}
#endif
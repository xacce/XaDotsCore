using System;
using UnityEngine;

namespace Core.Hybrid.Hybrid
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MinMaxSliderFloat2Attribute : PropertyAttribute
    {
        public float MinLimit;
        public float MaxLimit;

        public MinMaxSliderFloat2Attribute(float minLimit, float maxLimit)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
    }
}
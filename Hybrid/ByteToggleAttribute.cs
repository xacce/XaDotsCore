namespace Core.Hybrid.Hybrid
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field)]
    public class ByteToggleAttribute : PropertyAttribute
    {
        public string Label;

        public ByteToggleAttribute(string label = null)
        {
            Label = label;
        }
    }
}

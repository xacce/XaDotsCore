using System;
using UnityEngine;

namespace Core.Hybrid.Hybrid
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class UlongFlagsAttribute : PropertyAttribute
    {
        public Type EnumType { get; }

        public UlongFlagsAttribute(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                Debug.LogError("UlongFlagsAttribute must be used with an enum type.");
                return;
            }

            EnumType = enumType;
        }
    }

}
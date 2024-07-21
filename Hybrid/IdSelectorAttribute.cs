#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Core.Hybrid
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IdSelectorAttribute : PropertyAttribute
    {
        public IdSelectorAttribute(Type t)
        {
            this.t = t;
        }
        public Type t { get; set; }
    }
}
#endif
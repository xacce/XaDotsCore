using System;
using UnityEngine;

namespace Core.Hybrid
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class PickIdAttribute : PropertyAttribute
    {
        public PickIdAttribute(Type t)
        {
            this.t = t;
        }
        public Type t { get; set; }
    }
}
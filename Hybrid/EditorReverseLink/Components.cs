#if UNITY_EDITOR


using Unity.Entities;
using UnityEngine;

namespace Core.Hybrid.Hybrid.EditorReverseLink
{
    public partial struct EditorReverseLink : IComponentData
    {
        public UnityObjectRef<GameObject> go;
    }
}

#endif
#if UNITY_EDITOR
using Core.Runtime;
using Unity.Entities;
using UnityEngine;

namespace Core.Hybrid.Authoring
{
    public class DynamicObjectVelocityAuthoring : MonoBehaviour
    {
        private class DynamicObjectVelocityBaker : Baker<DynamicObjectVelocityAuthoring>
        {
            public override void Bake(DynamicObjectVelocityAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<DynamicObjectVelocity>(e);
            }
        }
    }
}
#endif
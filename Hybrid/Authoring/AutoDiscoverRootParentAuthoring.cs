using Core.Runtime;
using Unity.Entities;
using UnityEngine;

namespace Core.Hybrid.Hybrid.Authoring
{
    [DisallowMultipleComponent]
    public class AutoDiscoverRootParentAuthoring:MonoBehaviour
    {
        private class AutoDiscoverRootParentBaker : Baker<AutoDiscoverRootParentAuthoring>
        {
            public override void Bake(AutoDiscoverRootParentAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.None);
                AddComponent<AutoDiscoverRootParentTag>(e);
            }
        }
    }
}
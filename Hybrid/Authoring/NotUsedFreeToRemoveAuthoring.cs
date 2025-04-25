using Core.Runtime;
using Unity.Entities;
using UnityEngine;

namespace Core.Hybrid
{
    public class NotUsedFreeToRemoveAuthoring : MonoBehaviour
    {
        private class TriggerableBaker : Baker<NotUsedFreeToRemoveAuthoring>
        {
            public override void Bake(NotUsedFreeToRemoveAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.WorldSpace);
                AddComponent<NotUsedFreeToRemove>(e);
            }
        }
    }
}
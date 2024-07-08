using Core.Runtime;
using Unity.Entities;
using UnityEngine;

namespace Core.Hybrid
{
    public class TriggerableAuthoring : MonoBehaviour
    {
        private class TriggerableBaker : Baker<TriggerableAuthoring>
        {
            public override void Bake(TriggerableAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.None);
                AddComponent<Triggerable>(e);
            }
        }
    }
}
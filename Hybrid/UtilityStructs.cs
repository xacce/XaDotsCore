#if UNITY_EDITOR && UNITY_PHYSICS_CUSTOM

using System;
using Unity.Physics;
using Unity.Physics.Authoring;

namespace Core.Hybrid
{
    [Serializable]
    public struct DotsPhysicsMask
    {
        public PhysicsCategoryTags belongs;
        public PhysicsCategoryTags collide;

        public CollisionFilter AsFilter()
        {
            return new CollisionFilter
                { BelongsTo = belongs.Value, CollidesWith = collide.Value };
        }
    }
}

#endif
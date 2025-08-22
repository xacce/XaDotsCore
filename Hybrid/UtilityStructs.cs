#if UNITY_PHYSICS_CUSTOM

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
        public static explicit operator CollisionFilter(DotsPhysicsMask mask)
        {
            return mask.AsFilter();
        }
        
        public ulong Pack()
        {
            return ((ulong)belongs.Value << 32) | collide.Value;
        }

        public static CollisionFilter Unpack(ulong packed)
        {
            return new CollisionFilter
            {
                CollidesWith = (uint)(packed & 0xFFFFFFFF),
                BelongsTo = (uint)(packed >> 32),
            };
        }
    }
}


#endif
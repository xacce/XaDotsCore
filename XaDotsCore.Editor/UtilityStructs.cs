#if UNITY_EDITOR 
using System;
#if UNITY_PHYSICS_CUSTOM
using Unity.Physics;
using Unity.Physics.Authoring;
#endif

namespace XaDotsCore.Editor
{
#if UNITY_PHYSICS_CUSTOM
    namespace DotsCore.Utils.Components
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
}
#endif
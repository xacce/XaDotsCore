using System;
using Unity.Physics;
using Unity.Physics.Authoring;

namespace XaDotsCore.Editor
{
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
}
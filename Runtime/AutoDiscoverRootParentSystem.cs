using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Core.Runtime
{
    [BurstCompile]
    public partial struct AutoDiscoverRootParentSystem : ISystem
    {
        private Lookups _lookups;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _lookups = new Lookups(ref state);
            state.RequireForUpdate<AutoDiscoverRootParentTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _lookups.Update(ref state);
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (_, entity) in SystemAPI.Query<AutoDiscoverRootParentTag>().WithEntityAccess())
            {
                var baseEntity = entity;
                var chainEntity = entity;
                while (true)
                {
                    if (!_lookups.parentRo.TryGetComponent(chainEntity,out var parent))
                    {
                        ecb.RemoveComponent<AutoDiscoverRootParentTag>(baseEntity);
                        ecb.AddComponent(baseEntity,new RootParent(){value = chainEntity});
                        break;
                    }

                    chainEntity = parent.Value;
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        internal struct Lookups
        {
            [ReadOnly] public ComponentLookup<Parent> parentRo;

            public Lookups(ref SystemState state) : this()
            {
                parentRo = state.GetComponentLookup<Parent>();
            }

            [BurstCompile]
            public void Update(ref SystemState state)
            {
                parentRo.Update(ref state);
            }
        }
    }
}
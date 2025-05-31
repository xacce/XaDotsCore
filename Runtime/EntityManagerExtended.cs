using Unity.Collections;
using Unity.Entities;

namespace Core.Runtime
{
    public static class EntityManagerExtended
    {
        public static bool TryGetComponent<T>(this EntityManager em, Entity entity, out T data) where T : unmanaged, IComponentData
        {
            if (em.HasComponent<T>(entity))
            {
                data = em.GetComponentData<T>(entity);
                return true;
            }

            data = default;
            return false;
        }
    }

    public static class SystemStateExtended
    {
        public static T GetSingleton<T>(this ref SystemState state) where T : unmanaged, IComponentData
        {
            var q = state.GetEntityQuery(ComponentType.ReadOnly<T>());
            return q.GetSingleton<T>();
        }

        public static DynamicBuffer<TY> GetSingletonBufferByComponent<T, TY>(this ref SystemState state)
            where T : unmanaged, IComponentData where TY : unmanaged, IBufferElementData
        {
            var nt = new NativeArray<ComponentType>(2, Allocator.Temp)
            {
                [0] = ComponentType.ReadOnly<T>(),
                [1] = ComponentType.ReadOnly<TY>(),
            };
            var q = state.GetEntityQuery(nt);
            return q.GetSingletonBuffer<TY>();
        }

        public static TY GetSingletonComponentByComponent<TBy, TY>(this ref SystemState state) where TBy : unmanaged, IComponentData where TY : unmanaged, IComponentData
        {
            var nt = new NativeArray<ComponentType>(2, Allocator.Temp)
            {
                [0] = ComponentType.ReadOnly<TBy>(),
                [1] = ComponentType.ReadOnly<TY>(),
            };
            var q = state.GetEntityQuery(nt);
            return q.GetSingleton<TY>();
        }

        public static Entity GetSingletonEntity<T>(this ref SystemState state) where T : unmanaged, IComponentData
        {
            var q = state.GetEntityQuery(ComponentType.ReadOnly<T>());
            return q.GetSingletonEntity();
        }

        public static ref T GetSingletonRW<T>(this ref SystemState state) where T : unmanaged, IComponentData
        {
            var q = state.GetEntityQuery(ComponentType.ReadOnly<T>());
            return ref q.GetSingletonRW<T>().ValueRW;
        }
    }
}
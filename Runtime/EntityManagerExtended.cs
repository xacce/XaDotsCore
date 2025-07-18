﻿using Unity.Collections;
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

        public static bool TryGetBuffer<T>(this EntityManager em, Entity entity, out DynamicBuffer<T> data) where T : unmanaged, IBufferElementData
        {
            if (em.HasBuffer<T>(entity))
            {
                data = em.GetBuffer<T>(entity);
                return true;
            }

            data = default;
            return false;
        }

        //Do not call inside runtime loop. Only for initialization purposes
        public static Entity GetSingletonEntityAndForget<T>(this EntityManager state) where T : unmanaged, IComponentData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAll<T>().Build(state);
            var entity = q.GetSingletonEntity();
            q.Dispose();
            return entity;
        }

        public static bool TryGetSingletonEntityAndForget<T>(this EntityManager state,out Entity e) where T : unmanaged, IComponentData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAll<T>().Build(state);
            if (q.TryGetSingletonEntity<T>(out e))
            {
                q.Dispose();
                return true;
            }

            return false;
        }

        //Do not call inside runtime loop. Only for initialization purposes
        public static T GetSingletonDataAndForget<T>(this EntityManager state) where T : unmanaged, IComponentData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAll<T>().Build(state);
            var data = q.GetSingleton<T>();
            q.Dispose();
            return data;
        }

        public static T GetSingletonDataFromSystemAndForget<T>(this EntityManager state) where T : unmanaged, IComponentData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAll<T>().WithOptions(EntityQueryOptions.IncludeSystems).Build(state);
            var data = q.GetSingleton<T>();
            q.Dispose();
            return data;
        }

        //Do not call inside runtime loop. Only for initialization purposes
        public static Entity GetSingletonBufferEntityAndForget<T>(this EntityManager state) where T : unmanaged, IBufferElementData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAll<T>().Build(state);
            var entity = q.GetSingletonEntity();
            q.Dispose();
            return entity;
        }

        public static DynamicBuffer<T> GetSingletonBufferAndForget<T>(this EntityManager state) where T : unmanaged, IBufferElementData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAllRW<T>().Build(state);
            var entity = q.GetSingletonBuffer<T>();
            q.Dispose();
            return entity;
        }

        public static void SlowUpdateOrCreateSingletonData<T>(this EntityManager state, T data) where T : unmanaged, IComponentData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAllRW<T>().Build(state);
            if (q.TryGetSingletonRW(out RefRW<T> rw))
            {
                rw.ValueRW = data;
            }
            else
            {
                state.CreateSingleton(data);
            }

            q.Dispose();
        }

        public static void SlowReplaceOrCreateSingletonBuffer<T>(this EntityManager state, NativeArray<T> data) where T : unmanaged, IBufferElementData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAllRW<T>().Build(state);
            DynamicBuffer<T> buffer;
            if (q.TryGetSingletonBuffer(out buffer, false))
            {
                buffer.Clear();
            }
            else
            {
                buffer = state.GetBuffer<T>(state.CreateSingletonBuffer<T>());
            }

            buffer.AddRange(data);

            q.Dispose();
        }

        public static void SlowReplaceOrCreateSingletonBuffer<T>(this EntityManager state, T[] data) where T : unmanaged, IBufferElementData
        {
            var q = new EntityQueryBuilder(Allocator.Temp).WithAllRW<T>().Build(state);
            DynamicBuffer<T> buffer;
            if (q.TryGetSingletonBuffer(out buffer, false))
            {
                buffer.Clear();
            }
            else
            {
                buffer = state.GetBuffer<T>(state.CreateSingletonBuffer<T>());
            }

            buffer.CopyFrom(data);

            q.Dispose();
        }
    }

    public static class SystemStateExtended
    {
        public static T GetSingleton<T>(this ref SystemState state) where T : unmanaged, IComponentData
        {
            var q = state.GetEntityQuery(ComponentType.ReadOnly<T>());
            return q.GetSingleton<T>();
        }

        public static DynamicBuffer<T> GetSingletonBuffer<T>(this ref SystemState state) where T : unmanaged, IBufferElementData
        {
            var q = state.GetEntityQuery(ComponentType.ReadOnly<T>());
            return q.GetSingletonBuffer<T>();
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
            var q = state.GetEntityQuery(ComponentType.ReadWrite<T>());
            return ref q.GetSingletonRW<T>().ValueRW;
        }
    }
}
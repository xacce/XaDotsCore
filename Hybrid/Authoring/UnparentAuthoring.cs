using Core.Runtime.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Core.Hybrid.Hybrid.Authoring
{
    struct UnparentBakingTemp : IComponentData
    {
       
    }


    [DisallowMultipleComponent]
    public class UnparentAuthoring : MonoBehaviour
    {
        class _ : Baker<UnparentAuthoring>
        {
            public override void Bake(UnparentAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.None);
                AddComponent<UnparentBakingTemp>(e);
            }
        }
    }

    [WorldSystemFilter(WorldSystemFilterFlags.Editor)]
    [UpdateInGroup(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(ParentSystem))]
    partial struct UnparentSystem : ISystem
    {
        //Remove parents while editor works
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (parent, entity) in SystemAPI.Query<Parent>().WithEntityAccess().WithAll<UnparentBakingTemp>().WithOptions(EntityQueryOptions.IncludePrefab))
            {
                Debug.Log($"Adslkajsldkjaslkdjaslkdjaslkdjaskld: {parent.Value}");
                if (!SystemAPI.HasBuffer<Child>(parent.Value) || !SystemAPI.HasBuffer<LinkedEntityGroup>(parent.Value)) continue;
                Debug.Log($"Adslkajsldkjaslkdjaslkdjaslkdjaskld: {entity}");
                var child = SystemAPI.GetBuffer<Child>(parent.Value).Reinterpret<Entity>();

                var linked = SystemAPI.GetBuffer<LinkedEntityGroup>(parent.Value).Reinterpret<Entity>();
                child.Remove(entity);
                linked.Add(entity);
                ecb.RemoveComponent<Parent>(entity);
                ecb.RemoveComponent<PreviousParent>(entity);
                ecb.RemoveComponent<UnparentBakingTemp>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    [UpdateInGroup(typeof(PostBakingSystemGroup))]
    partial struct UnparentSystemB : ISystem
    {
        //Remove all parent data while baking (unity recreate hier after instantiation)
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (parent, entity) in SystemAPI.Query<Parent>().WithEntityAccess().WithAll<UnparentBakingTemp>().WithOptions(EntityQueryOptions.IncludePrefab))
            {
                ecb.RemoveComponent<Parent>(entity);
                ecb.RemoveComponent<PreviousParent>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
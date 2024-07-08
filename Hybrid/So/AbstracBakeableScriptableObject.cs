#if UNITY_EDITOR
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Core.Hybrid
{
    public abstract class BakedScriptableObject<T> : ScriptableObject where T : unmanaged
    {
        public BlobAssetReference<T> Bake(IBaker baker)
        {
            BlobBuilder builder = new BlobBuilder(Allocator.Temp);
            ref T definition = ref builder.ConstructRoot<T>();

            Bake(ref definition, ref builder);

            BlobAssetReference<T> blobReference = builder.CreateBlobAssetReference<T>(Allocator.Persistent);
            baker.AddBlobAsset(ref blobReference, out var hash);
            builder.Dispose();

            baker.DependsOn(this);

            return blobReference;
        }
        public abstract void Bake(ref T data, ref BlobBuilder blobBuilder);
    }
}

#endif
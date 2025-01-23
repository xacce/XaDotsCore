using System;
using System.Diagnostics;
using Unity.Entities;
using Object = UnityEngine.Object;

namespace Core.Runtime
{
    public class DotsValidation
    {
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public static void ThrowInvalidate<T>(BlobAssetReference<T> rf) where T : unmanaged
        {
            if (!rf.IsCreated) throw new Exception($"Blob asset not created. Type: {typeof(T)}");
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public static void ThrowInvalidate<T>(ref UnityObjectRef<T> rf) where T : Object
        {
            if (!rf.IsValid()) throw new Exception($"Invalid unity object ref. Type: {typeof(T)}.");
        }
    }
}
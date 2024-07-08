using System;
using System.Collections.Generic;
using UnityEditor;
using Object = UnityEngine.Object;

# if UNITY_EDITOR
namespace Core.Hybrid
{
    public static class Helpers
    {
        public static IEnumerable<T> FindAllAssetsByInterface<T>()
        {
            var allSos = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (var guid in allSos)
            foreach (var subAsset in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(guid)))
                if (subAsset is T subAssetT)
                    yield return subAssetT;
        }

        public static IEnumerable<T> FindAllAssetsByType<T>(bool allowSubAssets = true) where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var t in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) yield return asset;
            }
            // if (allowSubAssets)
            // {
            //     //tyty unity for searching not working for sub assets, fuck u.
            //     var allGuid = AssetDatabase.FindAssets("t:ScriptableObject");
            //     foreach (var guid in allGuid)
            //     foreach (var subAsset in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(guid)))
            //         if (subAsset is T subAssetT)
            //             yield return subAssetT;
            // }
        }
        public static IEnumerable<Object> FindAllAssetsByType(Type type)
        {
            var guids = AssetDatabase.FindAssets($"t:{type}");
            foreach (var t in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                if (asset != null) yield return asset;
            }
        }
    }
}
#endif
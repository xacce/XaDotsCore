// #if UNITY_EDITOR
//
// using System;
// using UnityEditor;
// using UnityEngine;
// using System.IO;
// using System.Text.RegularExpressions;
//
// namespace Core.Hybrid.Hybrid
// {
//     public class PrefabVariantUtils
//     {
//         [MenuItem("Assets/Insert Variant In Chain", true)]
//         private static bool ValidateInsertVariant()
//         {
//             GameObject selected = Selection.activeObject as GameObject;
//             if (selected == null) return false;
//
//
//             var prefabType = PrefabUtility.GetPrefabAssetType(selected);
//             Debug.Log(prefabType);
//             return prefabType == PrefabAssetType.Variant;
//         }
//
//         [MenuItem("Assets/Insert Variant In Chain")]
//         private static void InsertVariantInChain()
//         {
//             GameObject childVariant = Selection.activeObject as GameObject;
//             if (childVariant == null) return;
//
//             string childPath = AssetDatabase.GetAssetPath(childVariant);
//
//             GameObject parent = PrefabUtility.GetCorrespondingObjectFromSource(childVariant);
//             if (parent == null)
//             {
//                 Debug.LogError("Selected prefab is not a variant or has no parent.");
//                 return;
//             }
//
//             string newVariantPath = AssetDatabase.GenerateUniqueAssetPath(Path.GetDirectoryName(childPath) + "/InsertedMiddleVariant.prefab");
//
//             GameObject instantiated = (GameObject)PrefabUtility.InstantiatePrefab(parent);
//             GameObject middleVariant = PrefabUtility.SaveAsPrefabAsset(instantiated, newVariantPath);
//             GameObject.DestroyImmediate(instantiated);
//
//             AssetDatabase.SaveAssets();
//             AssetDatabase.Refresh();
//
//             // Получаем GUID нового варианта
//             string newVariantMetaPath = newVariantPath + ".meta";
//             var lines = File.ReadAllLines(newVariantMetaPath);
//             string guid = String.Empty;
//             foreach (var line in lines)
//             {
//                 if (line.StartsWith("guid: "))
//                 {
//                     guid = line.Replace("guid: ", "").Trim();
//                     break;
//                 }
//             }
//
//             Debug.Log($"New variant guid: {guid}");
//             if (String.IsNullOrEmpty(guid))
//             {
//                 Debug.LogError("Failed to get guid of new variant");
//                 return;
//             }
//
//             string prefabText = File.ReadAllText(childPath);
//
//             prefabText = Regex.Replace(prefabText,
//                 @"(m_SourcePrefab: {fileID: \d+, guid: )([a-f0-9]+)(, type: 3})",
//                 match => match.Groups[1].Value + guid + match.Groups[3].Value);
//             File.WriteAllText(childPath, prefabText);
//
//             AssetDatabase.ImportAsset(childPath, ImportAssetOptions.ForceUpdate);
//
//             Debug.Log($"✅ Inserted");
//         }
//     }
// }
// #endif
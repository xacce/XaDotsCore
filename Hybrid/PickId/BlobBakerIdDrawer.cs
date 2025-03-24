#if UNITY_EDITOR
using System;
using Core.Hybrid;
using UnityEditor;
using UnityEngine;

namespace GameReady.Ailments.Editor
{

    [CustomPropertyDrawer(typeof(PickIdAttribute))]
    public class PickIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type = (attribute as PickIdAttribute).t;
            if (GUI.Button(new Rect(position.x, position.y, 32f, position.height), "Pick"))
            {
                Pick(type,property);
            }

            EditorGUI.PropertyField(new Rect(position.x + 32, position.y, position.width - 32, position.height), property);


        }

        public static void Pick(Type type,SerializedProperty prop)
        {
            AssetDatabase.SaveAssets();
            var last = 0;
            foreach (var asset in Helpers.FindAllAssetsByType(type))
            {
                if (asset is IUniqueIdProvider idProvider)
                {
                    if (idProvider.id > last) last = idProvider.id;
                }
                else
                {
                    Debug.LogError($"U are trying to extract id from some object, but u didnt add IUniqueIdProvider interface. {asset} ", asset);
                }
            }
            prop.intValue = last + 1;
        }
    }
}
#endif
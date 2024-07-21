#if UNITY_EDITOR
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
                Debug.Log("KEKWAITTT");
                property.intValue = last + 1;
            }

            EditorGUI.PropertyField(new Rect(position.x + 32, position.y, position.width - 32, position.height), property);


        }

    }
}
#endif
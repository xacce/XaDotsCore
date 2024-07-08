#if UNITY_EDITOR
using Core.Hybrid;
using UnityEditor;
using UnityEngine;

namespace GameReady.Ailments.Editor
{
    [CustomPropertyDrawer(typeof(BlobBakerIdAttribute))]
    public class BlobBakerIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type = (attribute as BlobBakerIdAttribute).t;
            var currentValue = property.intValue;
            Object currentObject = null;
            if (currentValue >= 0)
            {
                foreach (var ailmentBlobBaked in Core.Hybrid.Helpers.FindAllAssetsByType(type))
                {
                    if ((ailmentBlobBaked as IUniqueBlobSoBaker).id == currentValue)
                    {
                        currentObject = ailmentBlobBaked;
                        break;
                    }
                }
            }
            
            var objectValue = EditorGUI.ObjectField(new Rect(position.x, position.y, position.width - 16f, position.height), label, currentObject, type, false);
            if (objectValue is not IUniqueBlobSoBaker un)
            {
                Debug.LogError($"Attribute: {attribute}, property: {property.displayName} is not a IUniqueBlobSoBaker interface ");
                return;
            }
            if (objectValue != null)
            {
                property.intValue = un.id;
            }
        }

    }
}
#endif
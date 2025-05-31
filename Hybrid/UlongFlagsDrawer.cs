using System;
using UnityEditor;
using UnityEngine;

namespace Core.Hybrid.Hybrid
{
    [CustomPropertyDrawer(typeof(UlongFlagsAttribute))]
    public class UlongFlagsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            UlongFlagsAttribute flagsAttribute = (UlongFlagsAttribute)attribute;
            Type enumType = flagsAttribute.EnumType;

            ulong currentValue = (ulong)property.longValue;
            Array enumValues = Enum.GetValues(enumType);
            string[] enumNames = Enum.GetNames(enumType);

            EditorGUI.BeginProperty(position, label, property);
            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                float lineHeight = EditorGUIUtility.singleLineHeight + 2f;
                for (int i = 0; i < enumValues.Length; i++)
                {
                    ulong flagValue = Convert.ToUInt64(enumValues.GetValue(i));
                    if (flagValue == 0) continue; 

                    bool isSet = (currentValue & flagValue) != 0;
                    Rect toggleRect = new Rect(position.x, position.y + (i + 1) * lineHeight, position.width, EditorGUIUtility.singleLineHeight);
                    bool newIsSet = EditorGUI.ToggleLeft(toggleRect, enumNames[i], isSet);

                    if (newIsSet != isSet)
                    {
                        if (newIsSet)
                            currentValue |= flagValue;
                        else
                            currentValue &= ~flagValue;
                    }
                }

                // Optional: handle "None" toggle
                ulong noneValue = 0;
                if (Enum.IsDefined(enumType, (object)noneValue))
                {
                    Rect toggleRect = new Rect(position.x, position.y + (enumValues.Length + 1) * lineHeight, position.width, EditorGUIUtility.singleLineHeight);
                    bool noneSelected = currentValue == 0;
                    bool newNoneSelected = EditorGUI.ToggleLeft(toggleRect, "None", noneSelected);
                    if (newNoneSelected && !noneSelected)
                        currentValue = 0;
                }

                property.ulongValue = (ulong)currentValue;
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            UlongFlagsAttribute flagsAttribute = (UlongFlagsAttribute)attribute;
            int flagCount = Enum.GetValues(flagsAttribute.EnumType).Length;
            return (flagCount + 1) * (EditorGUIUtility.singleLineHeight + 2f);
        }
    }
}
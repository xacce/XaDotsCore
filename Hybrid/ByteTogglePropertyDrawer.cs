#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace Core.Hybrid.Hybrid
{
    [CustomPropertyDrawer(typeof(ByteToggleAttribute))]
    public class ByteTogglePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Проверим тип
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                return new Label("ByteToggle can only be used with byte fields.");
            }

            var toggle = new Toggle();

            var attr = attribute as ByteToggleAttribute;
            toggle.label = string.IsNullOrEmpty(attr?.Label) ? property.displayName : attr.Label;

            toggle.value = property.intValue != 0;

            toggle.RegisterValueChangedCallback(evt =>
            {
                property.intValue = evt.newValue ? 1 : 0;
                property.serializedObject.ApplyModifiedProperties();
            });

            return toggle;
        }
    }
}
#endif
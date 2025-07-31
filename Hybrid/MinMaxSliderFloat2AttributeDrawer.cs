#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Hybrid.Hybrid
{
    [CustomPropertyDrawer(typeof(MinMaxSliderFloat2Attribute))]
    public class MinMaxSliderFloat2AttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {

            var attr = (MinMaxSliderFloat2Attribute)attribute;

            var xProp = property.FindPropertyRelative("x");
            var yProp = property.FindPropertyRelative("y");

            if (xProp == null || yProp == null)
                return new Label("float2 must have 'x' and 'y' fields.");

            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;

            // Заголовок
            var label = new Label(property.displayName);
            container.Add(label);

            // Горизонтальный блок: значение min | slider | значение max
            var sliderRow = new VisualElement();
            sliderRow.style.flexDirection = FlexDirection.Row;
            sliderRow.style.alignItems = Align.Center;
            // sliderRow.style.gap = 4;

            // Текстовые метки значений
            var minLabel = new Label(xProp.floatValue.ToString("0.###"));
            minLabel.style.minWidth = 40;
            var maxLabel = new Label(yProp.floatValue.ToString("0.###"));
            maxLabel.style.minWidth = 40;
            maxLabel.style.unityTextAlign = TextAnchor.MiddleRight;

            // Сам слайдер
            var slider = new MinMaxSlider(attr.MinLimit, attr.MaxLimit,attr.MinLimit, attr.MaxLimit)
            {
                value = new Vector2(xProp.floatValue, yProp.floatValue),
                style = { flexGrow = 1 }
            };

            // Синхронизация: слайдер → свойства + текст
            slider.RegisterValueChangedCallback(evt =>
            {
                xProp.floatValue = evt.newValue.x;
                yProp.floatValue = evt.newValue.y;
                property.serializedObject.ApplyModifiedProperties();

                minLabel.text = evt.newValue.x.ToString("0.###");
                maxLabel.text = evt.newValue.y.ToString("0.###");
            });

            sliderRow.Add(minLabel);
            sliderRow.Add(slider);
            sliderRow.Add(maxLabel);

            container.Add(sliderRow);
            return container;
        }
    }
}
#endif
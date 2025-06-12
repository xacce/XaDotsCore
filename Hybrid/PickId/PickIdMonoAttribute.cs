#if UNITY_EDITOR
using System.Linq;
using Core.Hybrid;
using Core.Hybrid.Hybrid.PickId;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(PickIdMonoAttribute))]
public class PickIdMonoDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        // Поле
        var field = new PropertyField(property);
        container.Add(field);

        // Кнопка
        var button = new Button(() =>
        {
            int maxId = FindMaxIdInPrefabs();
            property.intValue = maxId + 1;
            property.serializedObject.ApplyModifiedProperties();
        })
        {
            text = "Set Unique ID"
        };

        container.Add(button);
        return container;
    }

    private int FindMaxIdInPrefabs()
    {
        int maxId = 0;
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            var components = prefab.GetComponentsInChildren<MonoBehaviour>(true)
                .OfType<IUniqueIdProvider>();

            foreach (var provider in components)
            {
                    if (provider.id > maxId)
                        maxId = provider.id;
            }
        }

        return maxId;
    }
}
#endif
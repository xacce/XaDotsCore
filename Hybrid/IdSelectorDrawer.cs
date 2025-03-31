#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Hybrid;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameReady.Ailments.Editor
{
    [CustomPropertyDrawer(typeof(IdSelectorAttribute))]
    public class IdSelectorDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var field = new PropertyField(property);
            var button = new Button(() => OpenSearchWindow(property)) { text = "Search" };

            var horizontalLayout = new VisualElement { style = { flexDirection = FlexDirection.Row } };
            horizontalLayout.Add(field);
            horizontalLayout.Add(button);

            container.Add(horizontalLayout);
            return container;
        }

        // private void OpenSearchWindow(SerializedProperty property)
        // {
        //     var searchWindow = ScriptableObject.CreateInstance<SearchWindowProvider>();
        //     searchWindow.Configure((result) =>
        //     {
        //         property.objectReferenceValue = result;
        //         property.serializedObject.ApplyModifiedProperties();
        //         return true;
        //     }, fieldInfo.GetCustomAttribute<IdSelectorAttribute>().t);
        //
        //     SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), searchWindow);
        // }  

        private void OpenSearchWindow(SerializedProperty property)
        {
            var attribute = fieldInfo.GetCustomAttribute<IdSelectorAttribute>();
            // var searchContext = SearchService.CreateContext("asset", $"t:{attribute.t}");

            SearchService.ShowObjectPicker((item, cancelled) =>
            {
                if (cancelled) return;
                if (item is IUniqueIdProvider iud)
                {
                    property.intValue = iud.id;
                    property.serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    Debug.LogError($"Item {item} is not a {attribute.t} and implement IUniqueIdProvider");
                }
            }, o => { }, $"t:{attribute.t.Name}", attribute.t.ToString(),attribute.t);
        }
    }

    public class SearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private Func<UnityEngine.Object, bool> onSelect;
        private Type searchType;

        public void Configure(Func<UnityEngine.Object, bool> onSelect, Type searchType)
        {
            this.onSelect = onSelect;
            this.searchType = searchType;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent("Select Object"), 0) };

            var assets = AssetDatabase.FindAssets("t:" + searchType.Name);
            foreach (var guid in assets)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, searchType);
                if (obj != null)
                {
                    tree.Add(new SearchTreeEntry(new GUIContent(obj.name)) { level = 1, userData = obj });
                }
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            return onSelect?.Invoke(entry.userData as UnityEngine.Object) ?? false;
        }
    }

    // [CustomPropertyDrawer(typeof(IdSelectorAttribute))]
    // public class IdSelectorDrawer : PropertyDrawer
    // {
    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //         var type = (attribute as IdSelectorAttribute).t;
    //         var currentValue = property.intValue;
    //         Object currentObject = null;
    //         if (currentValue >= 0)
    //         {
    //             foreach (var ailmentBlobBaked in Core.Hybrid.Helpers.FindAllAssetsByType(type))
    //             {
    //                 if ((ailmentBlobBaked as IUniqueIdProvider).id == currentValue)
    //                 {
    //                     currentObject = ailmentBlobBaked;
    //                     break;
    //                 }
    //             }
    //         }
    //         
    //         var objectValue = EditorGUI.ObjectField(new Rect(position.x, position.y, position.width - 16f, position.height), label, currentObject, type, false);
    //         if (objectValue is not IUniqueIdProvider un)
    //         {
    //             return;
    //         }
    //         if (objectValue != null)
    //         {
    //             property.intValue = un.id;
    //         }
    //     }
    //
    // }
}
#endif
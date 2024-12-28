#if UNITY_EDITOR
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Core.Hybrid.Hybrid.EditorReverseLink
{
    public class EditorReverseLinkAuthoring : MonoBehaviour
    {
        private class EditorReverseLinkBaker : Baker<EditorReverseLinkAuthoring>
        {
            public override void Bake(EditorReverseLinkAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.None);
                AddComponent(e, new EditorReverseLink { go = authoring.gameObject });
            }
        }
    }
}

#endif
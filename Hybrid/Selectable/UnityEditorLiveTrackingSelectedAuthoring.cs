#if UNITY_EDITOR
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace Selectable
{
    public struct UnityEditorLiveTrackingSelected : IComponentData
    {
    }

    public class UnityEditorLiveTrackingSelectedAuthoring : MonoBehaviour
    {
        [SerializeField] private bool _selected;

        public void Toggle(bool status)
        {
            _selected = status;
            EditorUtility.SetDirty(this);
        }

        private class LiveKekBaker : Baker<UnityEditorLiveTrackingSelectedAuthoring>
        {
            public override void Bake(UnityEditorLiveTrackingSelectedAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.None);
                if (authoring._selected)
                    AddComponent(e, new UnityEditorLiveTrackingSelected() { });
            }
        }
    }
}
#endif
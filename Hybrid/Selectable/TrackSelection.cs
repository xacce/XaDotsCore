#if UNITY_EDITOR
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;

namespace Selectable
{
    // public static class OhUnity
    // {
    //     
    //     private static bool focused;
    //
    //     [UnityEditor.Callbacks.DidReloadScripts]
    //     private static void ScriptsHasBeenReloaded()
    //     {
    //         SceneView.duringSceneGui += DuringSceneGui;
    //     }
    //
    //     private static void DuringSceneGui(SceneView sceneView)
    //     {
    //         Event e = Event.current;
    //
    //         if (e.type == EventType.KeyDown && e.keyCode == KeyCode.F)
    //         {
    //             focused = true;
    //         }
    //     }
    //     public static bool IsFocused()=>
    // }

    [ExecuteInEditMode]
    public class TrackSelection : MonoBehaviour
    {
        private UnityEditorLiveTrackingSelectedAuthoring _previous;

        private void OnEnable()
        {
            Selection.selectionChanged += OnChanged;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnChanged;
        }

        private void Update()
        {
            if (Application.isPlaying) DirtyMechanics();
        }

        private void OnChanged()
        {
            var active = Selection.activeTransform;
            if (active != null && active.TryGetComponent(out UnityEditorLiveTrackingSelectedAuthoring live))
            {
                if (_previous != null && _previous.transform != null) _previous.Toggle(false);
                _previous = live;
                live.Toggle(true);
            }
            else if (_previous != null)
            {
                _previous.Toggle(false);
                _previous = null;
            }
        }

        private void DirtyMechanics()
        {
            var last = SceneView.lastActiveSceneView;
            if(!last) return;
            var worlds = World.All;
            for (int i = 0; i < worlds.Count; i++)
            {
                var world = worlds[i];
                var q = world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<UnityEditorLiveTrackingSelected>(), ComponentType.ReadOnly<LocalToWorld>());
                var data = q.ToComponentDataArray<LocalToWorld>(Allocator.Temp);
                if (data.Length == 0) continue;

                last.LookAt(data[0].Position);
                break;
            }
        }
    }
}
#endif
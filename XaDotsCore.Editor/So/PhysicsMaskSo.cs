using Unity.Physics;
using UnityEngine;
using XaDotsCore.Editor.DotsCore.Utils.Components;

namespace XaDotsCore.Editor.So
{
    [CreateAssetMenu(menuName = "Xadots/So/Physics mask")]
    public class PhysicsMaskSo : ScriptableObject
    {
        [SerializeField] private DotsPhysicsMask mask_s;

        public CollisionFilter asFilter => mask_s.AsFilter();
        public DotsPhysicsMask mask => mask_s;
    }
}
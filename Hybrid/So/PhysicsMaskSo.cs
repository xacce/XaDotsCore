#if UNITY_EDITOR
using Core.Hybrid;
using Unity.Physics;
using UnityEngine;

//sss
namespace Core.Hybrid
{
    [CreateAssetMenu(menuName = "Xadots/So/Physics mask")]
    public class PhysicsMaskSo : ScriptableObject
    {
        [SerializeField] private DotsPhysicsMask mask_s;

        public CollisionFilter asFilter => mask_s.AsFilter();
        public DotsPhysicsMask mask => mask_s;
    }
}
#endif
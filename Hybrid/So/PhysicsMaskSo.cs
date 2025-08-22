#if UNITY_EDITOR && UNITY_PHYSICS_CUSTOM
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
    }
}
#endif
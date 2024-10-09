
#if UNITY_EDITOR && !XACCE_UTILITY_BAKERS_OFF &&  LOCALIZATION_PACKAGE_EXISTS
using Unity.Entities;
using UnityEngine.Localization.PropertyVariants;

namespace Core.Hybrid.UtilityBakers
{
     public class GameObjectLocalizerBaker : Baker<GameObjectLocalizer>
    {
        public override void Bake(GameObjectLocalizer authoring)
        {
            var e = GetEntity(TransformUsageFlags.None);
            AddComponentObject(e, authoring);
        }
    }
}
#endif
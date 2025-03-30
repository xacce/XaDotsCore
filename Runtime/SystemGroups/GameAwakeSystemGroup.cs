using Unity.Entities;

namespace Core.Runtime.SystemGroups
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
    [UpdateBefore(typeof(BeginInitializationEntityCommandBufferSystem))] //neeeeed twice for groups 
    public partial class GameAwakeSystemGroup : ComponentSystemGroup
    {
    }
}
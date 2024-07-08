using Unity.Entities;

namespace Core.Runtime.SystemGroups
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderFirst = true)]
    public partial class GameAwakeSystem : ComponentSystemGroup
    {

    }
}
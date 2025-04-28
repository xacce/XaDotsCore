using Unity.Entities;

namespace Core.Runtime.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial class GameBlobUpdateSystemGroup : ComponentSystemGroup
    {
    }
}
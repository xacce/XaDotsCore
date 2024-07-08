using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Core.Runtime
{
    public static class Utility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint GetUniqueUIntFromInt(int val)
        {
            return math.select((uint)val, (uint)(int.MaxValue) + (uint)(val), val <= 0);
        }
    }
}
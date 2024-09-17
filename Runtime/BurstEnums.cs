using System;
using System.Runtime.CompilerServices;

namespace Core.Runtime
{
    public static class BurstEnums
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectFlag(ref ulong to, in ulong trueValue, in ulong falseValue, in bool condition)
        {
            if (condition)
            {
                to |= trueValue;
                to &= ~(falseValue);
            }
            else
            {
                to |= falseValue;
                to &= ~(trueValue);
            }
        }    [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectFlag(ref int to, in int trueValue, in int falseValue, in bool condition)
        {
            if (condition)
            {
                to |= trueValue;
                to &= ~(falseValue);
            }
            else
            {
                to |= falseValue;
                to &= ~(trueValue);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectFlag(ref ulong to, in ulong value, in bool condition)
        {
            if (condition)
                to |= value;
            else
                to &= ~(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectFlag(ref int to, in int value, in bool condition)
        {
            if (condition)
                to |= value;
            else
                to &= ~(value);
        }

       
    }
}
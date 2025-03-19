using System;
using Unity.Collections;

namespace Core.Runtime
{
    public static class FixedListU
    {
        public static bool Contains<T>(this FixedList32Bytes<T> list, T value) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Equals(value))
                    return true;
            }

            return false;
        }

        public static bool Contains<T>(this FixedList64Bytes<T> list, T value) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Equals(value))
                    return true;
            }

            return false;
        }

        public static bool Contains<T>(this FixedList128Bytes<T> list, T value) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Equals(value))
                    return true;
            }

            return false;
        }

        public static bool Contains<T>(this FixedList512Bytes<T> list, T value) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Equals(value))
                    return true;
            }

            return false;
        } 
        
        public static bool Contains<T>(this FixedList4096Bytes<T> list, T value) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Equals(value))
                    return true;
            }

            return false;
        }
    }
}
using System;
using Unity.Entities;

namespace Core.Runtime.Collections
{
    public static class BlobArrayUtils
    {
       
        public static bool Contains<T>(this ref BlobArray<T> array, T item) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }
       
    }
}
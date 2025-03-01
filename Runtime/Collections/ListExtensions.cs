using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

//original -> BovineLabs.Common.Native
namespace Core.Runtime.Collections
{
    /// <summary>
    /// Extensions for Native Containers.
    /// </summary>
    public static class Extensions
    {
        public static unsafe void CopyToManaged<T>(
            this NativeArray<T> nativeSlice,
            T[] array)
            where T : struct
        {
            int byteLength = nativeSlice.Length * UnsafeUtility.SizeOf<T>();
            void* managedBuffer = UnsafeUtility.AddressOf(ref array[0]);
            void* nativeBuffer = nativeSlice.GetUnsafePtr();
            UnsafeUtility.MemCpy(managedBuffer, nativeBuffer, byteLength);
        }     
        public static unsafe void CopyToManaged<T>(
            this DynamicBuffer<T> data,
            T[] array)
            where T :unmanaged
        {
            var nativeSlice = data.AsNativeArray();
            int byteLength = nativeSlice.Length * UnsafeUtility.SizeOf<T>();
            void* managedBuffer = UnsafeUtility.AddressOf(ref array[0]);
            void* nativeBuffer = nativeSlice.GetUnsafePtr();
            UnsafeUtility.MemCpy(managedBuffer, nativeBuffer, byteLength);
        }

        /// <summary>
        /// Adds a native version of <see cref="List{T}.AddRange(IEnumerable{T})"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The <see cref="List{T}"/> to add to.</param>
        /// <param name="array">The native array to add to the list.</param>
        public static unsafe void AddRange<T>(this List<T> list, NativeArray<T> array)
            where T : unmanaged
        {
            AddRange(list, array, array.Length);
        }

        /// <summary>
        /// Adds a native version of <see cref="List{T}.AddRange(IEnumerable{T})"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The <see cref="List{T}"/> to add to.</param>
        /// <param name="array">The array to add to the list.</param>
        /// <param name="length">The length of the array to add to the list.</param>
        public static unsafe void AddRange<T>(this List<T> list, NativeArray<T> array, int length)
            where T : unmanaged
        {
            list.AddRange(array.GetUnsafeReadOnlyPtr(), length);
        }

        /// <summary>
        /// Adds a native version of <see cref="List{T}.AddRange(IEnumerable{T})"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The <see cref="List{T}"/> to add to.</param>
        /// <param name="nativeList">The native list to add to the list.</param>
        public static unsafe void AddRange<T>(this List<T> list, NativeList<T> nativeList)
            where T : unmanaged
        {
            list.AddRange(nativeList.GetUnsafePtr(), nativeList.Length);
        }

        /// <summary>
        /// Adds a native version of <see cref="List{T}.AddRange(IEnumerable{T})"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The <see cref="List{T}"/> to add to.</param>
        /// <param name="nativeSlice">The array to add to the list.</param>
        public static unsafe void AddRange<T>(this List<T> list, NativeSlice<T> nativeSlice)
            where T : unmanaged
        {
            list.AddRange(nativeSlice.GetUnsafeReadOnlyPtr(), nativeSlice.Length);
        }

        /// <summary>
        /// Adds a native version of <see cref="List{T}.AddRange(IEnumerable{T})"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The <see cref="List{T}"/> to add to.</param>
        /// <param name="dynamicBuffer">The dynamic buffer to add to the list.</param>
        public static unsafe void AddRange<T>(this List<T> list, DynamicBuffer<T> dynamicBuffer)
            where T : unmanaged
        {
            list.AddRange(dynamicBuffer.GetUnsafePtr(), dynamicBuffer.Length);
        }

        /// <summary>
        /// Adds a range of values to a list using a buffer;
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list to add the values to.</param>
        /// <param name="arrayBuffer">The buffer to add from.</param>
        /// <param name="length">The length of the buffer.</param>
        public static unsafe void AddRange<T>(this List<T> list, void* arrayBuffer, int length)
            where T : unmanaged
        {
            var index = list.Count;
            var newLength = index + length;

            // Resize our list if we require
            if (list.Capacity < newLength)
            {
                list.Capacity = newLength;
            }

            var items = NoAllocHelpers.ExtractArrayFromListT(list);
            var size = UnsafeUtility.SizeOf<T>();

            // Get the pointer to the end of the list
            var bufferStart = (IntPtr)UnsafeUtility.AddressOf(ref items[0]);
            var buffer = (byte*)(bufferStart + (size * index));

            UnsafeUtility.MemCpy(buffer, arrayBuffer, length * (long)size);

            NoAllocHelpers.ResizeList(list, newLength);
        }


       
    }

    public static class E
    {
        public static void CopyTo<T>(this IEnumerable<T> fr, BlobBuilderArray<T> t) where T : unmanaged
        {
            var enumerable = fr as T[] ?? fr.ToArray();
            for (int i = 0; i < enumerable.Count(); i++)
            {
                t[i] = enumerable.ElementAt(i);
            }
        }
    }
}
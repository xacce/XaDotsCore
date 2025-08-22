using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace Xacce.Core.Collections
{
    public interface IRotatorLeft<TLeft> where TLeft : unmanaged, IEquatable<TLeft>
    {
        public TLeft left { get; }
    }

    public class ArrayRotator<TLeft, TRight> where TLeft : unmanaged, IEquatable<TLeft> where TRight : IRotatorLeft<TLeft>
    {
        public delegate void Removed(TRight el);

        public delegate void Rebind(TLeft left, ref TRight right);

        public delegate TRight Add(TLeft left);

        private Removed _onRemoved;
        private Rebind _onRebind;
        private Add _onAdd;

        public ArrayRotator(Removed onRemoved, Rebind onRebind, Add onAdd)
        {
            _onRemoved = onRemoved;
            _onRebind = onRebind;
            _onAdd = onAdd;
        }

        public void Rotate(DynamicBuffer<TLeft> left, List<TRight> right)
        {
            Rotate(left.AsNativeArray(), right);
        }

        public void Rotate(NativeArray<TLeft> left, List<TRight> right)
        {
            var diff = left.Length - right.Count;
            var valid = Math.Min(left.Length, right.Count);
            for (int i = 0; i < valid; i++)
            {
                var leftEl = left[i];
                var rightEl = right[i];
                if (!leftEl.Equals(rightEl.left))
                {
                    _onRebind(leftEl, ref rightEl);
                }

                right[i] = rightEl;
            }

            if (diff > 0)
            {
                for (int i = right.Count; i < left.Length; i++)
                {
                    right.Add(_onAdd(left[i]));
                }
            }
            else
            {
                for (int i = left.Length; i < right.Count; i++)
                {
                    _onRemoved(right[i]);
                    right.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
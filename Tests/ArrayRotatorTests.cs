using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using Xacce.Core.Collections;

namespace Rotator.Tests
{
    [TestFixture]
    public class ArrayRotatorTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        struct Left : IEquatable<Left>
        {
            public int value;

            public bool Equals(Left other)
            {
                return value == other.value;
            }

            public override bool Equals(object obj)
            {
                return obj is Left other && Equals(other);
            }

            public override int GetHashCode()
            {
                return value;
            }
        }

        struct Right : IRotatorLeft<Left>
        {
            public Left left { get; set; }
        }

        [Test]
        public void ArrayRotatorTests_Dummy()
        {
            using var left = new NativeArray<Left>(0, Allocator.Temp);
            var right = new List<Right>();
            var dummyRotator = new ArrayRotator<Left, Right>((el) => { Assert.True(false); }, (Left left, ref Right right) => { Assert.True(false); }, (left) =>
            {
                Assert.True(false);
                return new Right { left = left };
            });
            dummyRotator.Rotate(left, right);
        }

        [Test]
        public void ArrayRotatorTests_PureAdd()
        {
            using var left = new NativeList<Left>(0, Allocator.Temp);
            left.Add(new Left { value = 1 });

            var right = new List<Right>();
            var dummyRotator = new ArrayRotator<Left, Right>((el) => { Assert.True(false); }, (Left left, ref Right right) => { Assert.True(false); }, (left) =>
            {
                Assert.True(true);
                return new Right { left = left };
            });
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 1);
            Assert.True(right[0].left.value.Equals(left[0].value));
            left.Add(new Left { value = 2 });
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 2);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[1].left.value.Equals(left[1].value));
        }

        [Test]
        public void ArrayRotatorTests_Rebind()
        {
            using var left = new NativeList<Left>(0, Allocator.Temp);
            left.Add(new Left { value = 1 });
            var right = new List<Right>();
            var dummyRotator = new ArrayRotator<Left, Right>((el) => { Assert.True(false); }, (Left left, ref Right right) =>
            {
                Debug.Log($"Rebind {left.value} {right.left.value}");
                Assert.True(left.value == 2);
                Assert.True(right.left.value == 1);
                right.left = left;
            }, (left) =>
            {
                Assert.True(true);
                return new Right { left = left };
            });
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 1);
            Assert.True(right[0].left.value.Equals(left[0].value));
            ref var f = ref left.ElementAt(0);
            f.value = 2;
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 1);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[0].left.value == 2);
        }

        [Test]
        public void ArrayRotatorTests_Remove()
        {
            using var left = new NativeList<Left>(0, Allocator.Temp);
            left.Add(new Left { value = 1 });
            left.Add(new Left { value = 2 });
            left.Add(new Left { value = 3 });
            var right = new List<Right>();
            var dummyRotator = new ArrayRotator<Left, Right>((el) => { Assert.True(true); }, (Left left, ref Right right) => { Assert.True(false); },
                (left) =>
                {
                    Assert.True(true);
                    return new Right { left = left };
                });
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 3);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[1].left.value.Equals(left[1].value));
            Assert.True(right[2].left.value.Equals(left[2].value));
            left.RemoveAt(2);
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 2);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[1].left.value.Equals(left[1].value));
            left.RemoveAt(1);
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 1);
            Assert.True(right[0].left.value.Equals(left[0].value));
            left.RemoveAt(0);
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 0);
        }

        [Test]
        public void ArrayRotatorTests_MassRemove()
        {
            using var left = new NativeList<Left>(0, Allocator.Temp);
            left.Add(new Left { value = 1 });
            left.Add(new Left { value = 2 });
            left.Add(new Left { value = 3 });
            var right = new List<Right>();
            var dummyRotator = new ArrayRotator<Left, Right>((el) => { Assert.True(true); }, (Left left, ref Right right) => { Assert.True(false); },
                (left) =>
                {
                    Assert.True(true);
                    return new Right { left = left };
                });
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 3);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[1].left.value.Equals(left[1].value));
            Assert.True(right[2].left.value.Equals(left[2].value));
            left.RemoveAt(2);
            left.RemoveAt(1);
            left.RemoveAt(0);
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 0);
        }

        [Test]
        public void ArrayRotatorTests_RemoveRebind()
        {
            using var left = new NativeList<Left>(0, Allocator.Temp);
            left.Add(new Left { value = 1 });
            left.Add(new Left { value = 2 });
            left.Add(new Left { value = 3 });
            var right = new List<Right>();
            var dummyRotator = new ArrayRotator<Left, Right>((el) => { Assert.True(true); }, (Left left, ref Right right) => { right.left = left; },
                (left) =>
                {
                    Assert.True(true);
                    return new Right { left = left };
                });
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 3);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[1].left.value.Equals(left[1].value));
            Assert.True(right[2].left.value.Equals(left[2].value));
            left.RemoveAt(1);
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 2);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[1].left.value.Equals(left[1].value));
            Assert.True(right[0].left.value == 1);
            Assert.True(right[1].left.value == 3);
        }

        [Test]
        public void ArrayRotatorTests_InsertRebind()
        {
            using var left = new NativeList<Left>(0, Allocator.Temp);
            left.Add(new Left { value = 1 });
            left.Add(new Left { value = 3 });
            var right = new List<Right>();
            var dummyRotator = new ArrayRotator<Left, Right>((el) => { Assert.True(true); }, (Left left, ref Right right) => { right.left = left; },
                (left) =>
                {
                    Assert.True(true);
                    return new Right { left = left };
                });
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 2);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[1].left.value.Equals(left[1].value));
            left.InsertRange(1, 1);
            ref var f = ref left.ElementAt(1);
            f.value = 2;
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 3);
            Assert.True(right[0].left.value.Equals(left[0].value));
            Assert.True(right[1].left.value.Equals(left[1].value));
            Assert.True(right[2].left.value.Equals(left[2].value));
            Assert.True(right[0].left.value == 1);
            Assert.True(right[1].left.value == 2);
            Assert.True(right[2].left.value == 3);
        }

        [Test]
        public void ArrayRotatorTests_Full()
        {
            using var left = new NativeList<Left>(0, Allocator.Temp);
            for (int i = 0; i < 100; i++)
            {
                left.Add(new Left { value = i });
            }

            var right = new List<Right>();
            var dummyRotator = new ArrayRotator<Left, Right>((el) => { }, (Left left, ref Right right) => { right.left = left; },
                (left) => { return new Right { left = left }; });
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == left.Length);
            Assert.True(right.Count == 100);
            for (int i = 0; i < 100; i++)
            {
                Assert.True(right[i].left.value.Equals(left[i].value));
            }

            for (int i = 0; i < 10; i++)
            {
                left.RemoveAt(0);
            }

            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 90);
            for (int i = 0; i < left.Length; i++)
            {
                Assert.True(right[i].left.value.Equals(left[i].value));
                Assert.True(right[i].left.value == 10 + i);
            }

            for (int i = 0; i < 10; i++)
            {
                left.RemoveAt(left.Length - 1);
            }

            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 80);
            for (int i = 0; i < left.Length; i++)
            {
                Assert.True(right[i].left.value.Equals(left[i].value));
                Assert.True(right[i].left.value == 10 + i);
            }

            left.InsertRange(0, 10);
            for (int i = 0; i < 10; i++)
            {
                ref var f = ref left.ElementAt(i);
                f.value = i;
            }

            for (int i = 0; i < 10; i++)
            {
                left.Add(new Left { value = 90 + i });
            }

            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == left.Length);
            Assert.True(right.Count == 100);
            for (int i = 0; i < 100; i++)
            {
                Assert.True(right[i].left.value.Equals(left[i].value));
            }

            left.Clear();
            dummyRotator.Rotate(left, right);
            Assert.True(right.Count == 0);
        }
    }
}
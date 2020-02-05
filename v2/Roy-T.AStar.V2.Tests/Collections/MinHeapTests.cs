using NUnit.Framework;
using Roy_T.AStar.V2.Collections;

namespace Roy_T.AStar.V2.Tests.Collections
{
    public sealed class MinHeapTest
    {
        [Test]
        public void ShouldSort__ReserveSortedInput()
        {
            var heap = new MinHeap<int>();
            heap.Insert(5);
            heap.Insert(4);
            heap.Insert(3);
            heap.Insert(2);
            heap.Insert(1);

            Assert.That(heap.Peek(), Is.EqualTo(1));
            Assert.That(heap.Extract(), Is.EqualTo(1));

            Assert.That(heap.Peek(), Is.EqualTo(2));
            Assert.That(heap.Extract(), Is.EqualTo(2));

            Assert.That(heap.Peek(), Is.EqualTo(3));
            Assert.That(heap.Extract(), Is.EqualTo(3));

            Assert.That(heap.Peek(), Is.EqualTo(4));
            Assert.That(heap.Extract(), Is.EqualTo(4));

            Assert.That(heap.Peek(), Is.EqualTo(5));
            Assert.That(heap.Extract(), Is.EqualTo(5));
        }

        [Test]
        public void ShouldSort__SortedInput()
        {
            var heap = new MinHeap<int>();
            heap.Insert(1);
            heap.Insert(2);
            heap.Insert(3);
            heap.Insert(4);
            heap.Insert(5);

            Assert.That(heap.Peek(), Is.EqualTo(1));
            Assert.That(heap.Extract(), Is.EqualTo(1));

            Assert.That(heap.Peek(), Is.EqualTo(2));
            Assert.That(heap.Extract(), Is.EqualTo(2));

            Assert.That(heap.Peek(), Is.EqualTo(3));
            Assert.That(heap.Extract(), Is.EqualTo(3));

            Assert.That(heap.Peek(), Is.EqualTo(4));
            Assert.That(heap.Extract(), Is.EqualTo(4));

            Assert.That(heap.Peek(), Is.EqualTo(5));
            Assert.That(heap.Extract(), Is.EqualTo(5));
        }

        [Test]
        public void ShouldSort__UnsortedInput()
        {
            var heap = new MinHeap<int>();
            heap.Insert(3);
            heap.Insert(2);
            heap.Insert(1);
            heap.Insert(5);
            heap.Insert(4);

            Assert.That(heap.Peek(), Is.EqualTo(1));
            Assert.That(heap.Extract(), Is.EqualTo(1));

            Assert.That(heap.Peek(), Is.EqualTo(2));
            Assert.That(heap.Extract(), Is.EqualTo(2));

            Assert.That(heap.Peek(), Is.EqualTo(3));
            Assert.That(heap.Extract(), Is.EqualTo(3));

            Assert.That(heap.Peek(), Is.EqualTo(4));
            Assert.That(heap.Extract(), Is.EqualTo(4));

            Assert.That(heap.Peek(), Is.EqualTo(5));
            Assert.That(heap.Extract(), Is.EqualTo(5));
        }

        [Test]
        public void ShouldSort__AfterRemoving()
        {
            var heap = new MinHeap<int>();
            heap.Insert(1);
            heap.Insert(2);
            heap.Insert(3);
            heap.Insert(4);
            heap.Insert(5);

            heap.Remove(4);

            Assert.That(heap.Extract(), Is.EqualTo(1));
            Assert.That(heap.Extract(), Is.EqualTo(2));
            Assert.That(heap.Extract(), Is.EqualTo(3));
            Assert.That(heap.Extract(), Is.EqualTo(5));
        }
    }
}

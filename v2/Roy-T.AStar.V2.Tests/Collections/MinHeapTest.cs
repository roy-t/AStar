using NUnit.Framework;
using Roy_T.AStar.V2.Collections;

namespace Roy_T.AStar.V2.Tests.Collections
{
    public sealed class MinHeapTest
    {
        [Test]
        public void ShouldSort__ReserveSortedInput()
        {
            var heap = new MinHeap();
            heap.Insert(5);
            heap.Insert(4);
            heap.Insert(3);
            heap.Insert(2);
            heap.Insert(1);

            Assert.That(heap.Peak().Value, Is.EqualTo(1));
            Assert.That(heap.Extract().Value, Is.EqualTo(1));

            Assert.That(heap.Peak().Value, Is.EqualTo(2));
            Assert.That(heap.Extract().Value, Is.EqualTo(2));

            Assert.That(heap.Peak().Value, Is.EqualTo(3));
            Assert.That(heap.Extract().Value, Is.EqualTo(3));

            Assert.That(heap.Peak().Value, Is.EqualTo(4));
            Assert.That(heap.Extract().Value, Is.EqualTo(4));

            Assert.That(heap.Peak().Value, Is.EqualTo(5));
            Assert.That(heap.Extract().Value, Is.EqualTo(5));
        }

        [Test]
        public void ShouldSort__SortedInput()
        {
            var heap = new MinHeap();
            heap.Insert(1);
            heap.Insert(2);
            heap.Insert(3);
            heap.Insert(4);
            heap.Insert(5);

            Assert.That(heap.Peak().Value, Is.EqualTo(1));
            Assert.That(heap.Extract().Value, Is.EqualTo(1));

            Assert.That(heap.Peak().Value, Is.EqualTo(2));
            Assert.That(heap.Extract().Value, Is.EqualTo(2));

            Assert.That(heap.Peak().Value, Is.EqualTo(3));
            Assert.That(heap.Extract().Value, Is.EqualTo(3));

            Assert.That(heap.Peak().Value, Is.EqualTo(4));
            Assert.That(heap.Extract().Value, Is.EqualTo(4));

            Assert.That(heap.Peak().Value, Is.EqualTo(5));
            Assert.That(heap.Extract().Value, Is.EqualTo(5));
        }

        [Test]
        public void ShouldSort__UnsortedInput()
        {
            var heap = new MinHeap();
            heap.Insert(3);
            heap.Insert(2);
            heap.Insert(1);
            heap.Insert(5);
            heap.Insert(4);

            Assert.That(heap.Peak().Value, Is.EqualTo(1));
            Assert.That(heap.Extract().Value, Is.EqualTo(1));

            Assert.That(heap.Peak().Value, Is.EqualTo(2));
            Assert.That(heap.Extract().Value, Is.EqualTo(2));

            Assert.That(heap.Peak().Value, Is.EqualTo(3));
            Assert.That(heap.Extract().Value, Is.EqualTo(3));

            Assert.That(heap.Peak().Value, Is.EqualTo(4));
            Assert.That(heap.Extract().Value, Is.EqualTo(4));

            Assert.That(heap.Peak().Value, Is.EqualTo(5));
            Assert.That(heap.Extract().Value, Is.EqualTo(5));
        }
    }
}

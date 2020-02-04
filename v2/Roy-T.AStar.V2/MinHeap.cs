using System.Collections.Generic;

namespace Roy_T.AStar.V2
{
    // C# Adaptation of a min heap built for C++ by Robin Thomas
    // Original source code at: https://github.com/robin-thomas/min-heap

    public sealed class MinHeap
    {
        private readonly List<MinHeapNode> Items;

        public MinHeap()
        {
            this.Items = new List<MinHeapNode>();
        }

        public int Count => this.Items.Count;

        public MinHeapNode Peek() => this.Items[0];

        public MinHeapNode Insert(INode pathNode, MinHeapNode cameFrom, IEdge cameVia, Duration timeSoFar, Duration expectedRemainingTime)
        {
            var heapNode = new MinHeapNode(pathNode, cameFrom, cameVia, timeSoFar, expectedRemainingTime);
            this.Items.Add(heapNode);
            this.Insert(heapNode);

            return heapNode;
        }

        private void Insert(MinHeapNode heapNode)
        {
            var index = this.Items.Count - 1;
            while (index > 0 && heapNode.ExpectedTotalTime < this.Items[Parent(index)].ExpectedTotalTime)
            {
                this.Items[index] = this.Items[Parent(index)];
                index = Parent(index);
            }

            this.Items[index] = heapNode;
        }

        public MinHeapNode Extract()
        {
            var node = this.Items[0];

            this.Items[0] = this.Items[this.Items.Count - 1];
            this.Items.RemoveAt(this.Items.Count - 1);

            this.Heapify(0);

            return node;
        }

        private void Heapify(int index)
        {
            var smallest = (LeftChild(index) < this.Items.Count && this.Items[LeftChild(index)].ExpectedTotalTime < this.Items[index].ExpectedTotalTime) ? LeftChild(index) : index;
            if (RightChild(index) < this.Items.Count && this.Items[RightChild(index)].ExpectedTotalTime < this.Items[smallest].ExpectedTotalTime)
            {
                smallest = RightChild(index);
            }

            if (smallest != index)
            {
                var temp = this.Items[smallest];
                this.Items[smallest] = this.Items[index];
                this.Items[index] = temp;

                this.Heapify(smallest);
            }
        }

        private static int Parent(int i) => (i - 1) / 2;
        private static int LeftChild(int i) => (2 * i) + 1;
        private static int RightChild(int i) => (2 * i) + 2;
    }
}

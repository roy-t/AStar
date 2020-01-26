using System.Collections.Generic;

namespace Roy_T.AStar.V2.Collections
{
    // For a good explanation of min heaps see: https://robin-thomas.github.io/min-heap/

    public sealed class MinHeap
    {
        private readonly List<MinHeapNode> Items;

        public MinHeap()
        {
            this.Items = new List<MinHeapNode>();
        }

        public int Count => this.Items.Count;

        public MinHeapNode Peak() => this.Items[0];

        public void Insert(float value)
        {
            var node = new MinHeapNode(value);
            this.Items.Add(node);

            var index = this.Items.Count - 1;
            while (index > 0 && node.Value < this.Items[Parent(index)].Value)
            {
                this.Items[index] = this.Items[Parent(index)];
                index = Parent(index);
            }

            this.Items[index] = node;
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
            var smallest = (LeftChild(index) < this.Items.Count && this.Items[LeftChild(index)].Value < this.Items[index].Value) ? LeftChild(index) : index;
            if (RightChild(index) < this.Items.Count && this.Items[RightChild(index)].Value < this.Items[smallest].Value)
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


    public sealed class MinHeapNode
    {
        public MinHeapNode(float value)
        {
            this.Value = value;
        }
        public float Value { get; }

        public override string ToString() => $"{this.Value}";
    }
}

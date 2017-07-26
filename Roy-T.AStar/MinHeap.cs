namespace RoyT.AStar
{   
    /// <summary>
    /// Heap which keeps the node with the minimal expected path cost on the head position
    /// </summary>
    internal sealed class MinHeap
    {
        private SearchNode head;      

        /// <summary>
        /// If the heap has a next element
        /// </summary>        
        public bool HasNext() => this.head != null;

        /// <summary>
        /// Pushes a node onto the heap        
        /// </summary>
        public void Push(SearchNode node)
        {
            // If the heap is empty, just add the item to the top
            if (this.head == null)
            {
                this.head = node;
            }
            else if (node.ExpectedCost < this.head.ExpectedCost)
            {
                node.NextListElement = this.head;
                this.head = node;
            }            
            else
            {
                var current = this.head;
                while (current.NextListElement != null && current.NextListElement.ExpectedCost <= node.ExpectedCost)
                {
                    current = current.NextListElement;
                }

                node.NextListElement = current.NextListElement;
                current.NextListElement = node;
            }
        }

        /// <summary>
        /// Pops a node from the heap, this node is always the node
        /// with the cheapest path cost
        /// </summary>
        public SearchNode Pop()
        {
            var top = this.head;
            this.head = this.head.NextListElement;

            return top;
        }
    }
}

namespace RoyT.AStar
{   
    internal sealed class MinHeap
    {
        private SearchNode head;

        public bool HasNext() => this.head != null;

        public void Push(SearchNode node)
        {
            // If the heap is empty, just add the item to the top
            if (this.head == null)
            {
                this.head = node;
            }
            // Insertion sort on cost           
            else
            {
                var current = this.head;
                while (current.NextListElement != null && current.NextListElement.Cost < node.Cost)
                {
                    current = current.NextListElement;
                }

                node.NextListElement = current.NextListElement;
                current.NextListElement = node;
            }
        }

        public SearchNode Pop()
        {
            var top = this.head;
            this.head = this.head.NextListElement;

            return top;
        }
    }
}

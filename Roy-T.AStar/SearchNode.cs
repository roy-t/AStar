namespace RoyT.AStar
{
    internal sealed class SearchNode
    {
        public SearchNode(Position position)
            : this(position, 0, 0) { }

        public SearchNode(Position position, double cost, double pathCost)
        {
            this.Position = position;
            this.Cost = cost;
            this.PathCost = pathCost;
        }

        public Position Position { get; }
        public double Cost { get; set; }
        public double PathCost { get; set; }

        public SearchNode Next { get; set; }
        public SearchNode NextListElement { get; set; }
    }
}

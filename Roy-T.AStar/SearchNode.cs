namespace RoyT.AStar
{
    /// <summary>
    /// Stores intermediate results in an A* path search
    /// </summary>
    internal sealed class SearchNode
    {
        public SearchNode(Position position)
            : this(position, 0, 0) { }

        public SearchNode(Position position, double expectedCost, double costSoFar)
        {
            this.Position     = position;
            this.ExpectedCost = expectedCost;
            this.CostSoFar    = costSoFar;
        }

        public Position Position { get; }
        public double ExpectedCost { get; set; }
        public double CostSoFar { get; set; }

        public SearchNode Next { get; set; }
        public SearchNode NextListElement { get; set; }
    }
}

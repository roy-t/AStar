namespace RoyT.AStar
{
    /// <summary>
    /// Stores intermediate results in an A* path search
    /// </summary>
    internal sealed class SearchNode
    {
        public SearchNode(Position position)
            : this(position, 0, 0) { }

        public SearchNode(Position position, float expectedCost, float costSoFar)
        {
            this.Position     = position;
            this.ExpectedCost = expectedCost;
            this.CostSoFar    = costSoFar;
        }

        public Position Position { get; }
        public float ExpectedCost { get; set; }
        public float CostSoFar { get; set; }

        public SearchNode Next { get; set; }
        public SearchNode NextListElement { get; set; }
    }
}

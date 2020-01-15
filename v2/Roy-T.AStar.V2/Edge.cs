namespace Roy_T.AStar.V2
{
    public sealed class Edge : IEdge
    {
        public Edge(INode start, INode end, float traversalCost)
        {
            this.Start = start;
            this.End = end;
            this.TraversalCost = traversalCost;
        }

        public float TraversalCost { get; }
        public INode Start { get; }
        public INode End { get; }
    }
}

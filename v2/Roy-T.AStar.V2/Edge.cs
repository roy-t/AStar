namespace Roy_T.AStar.V2
{
    public sealed class Edge : IEdge
    {
        public Edge(INode start, INode end, Velocity traversalVelocity)
        {
            this.Start = start;
            this.End = end;
            this.TraversalVelocity = traversalVelocity;
        }

        public Velocity TraversalVelocity { get; set; }
        public INode Start { get; }
        public INode End { get; }

        public override string ToString() => $"{this.Start} -> {this.End} @ {this.TraversalVelocity}";
    }
}

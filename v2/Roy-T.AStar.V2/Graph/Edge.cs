namespace Roy_T.AStar.V2.Graph
{
    public sealed class Edge : IEdge
    {
        private Velocity traversalVelocity;

        public Edge(INode start, INode end, Velocity traversalVelocity)
        {
            this.Start = start;
            this.End = end;
            this.TraversalVelocity = traversalVelocity;
        }

        public Velocity TraversalVelocity
        {
            get => this.traversalVelocity;
            set
            {
                this.traversalVelocity = value;
                this.TraversalDuration = MathUtil.ExpectedTime(this.Start.X, this.Start.Y, this.End.X, this.End.Y, value);
            }
        }

        public Duration TraversalDuration { get; private set; }

        public INode Start { get; }
        public INode End { get; }

        public override string ToString() => $"{this.Start} -> {this.End} @ {this.TraversalVelocity}";
    }
}

using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Graphs
{
    public sealed class Edge : IEdge
    {
        private Velocity traversalVelocity;

        public Edge(INode start, INode end, Velocity traversalVelocity)
        {
            this.Start = start;
            this.End = end;

            this.Distance = Distance.BeweenPositions(start.Position, end.Position);
            this.TraversalVelocity = traversalVelocity;
        }

        public Velocity TraversalVelocity
        {
            get => this.traversalVelocity;
            set
            {
                this.traversalVelocity = value;
                this.TraversalDuration = this.Distance / value;
            }
        }

        public Duration TraversalDuration { get; private set; }

        public Distance Distance { get; }

        public INode Start { get; }
        public INode End { get; }

        public override string ToString() => $"{this.Start} -> {this.End} @ {this.TraversalVelocity}";
    }
}

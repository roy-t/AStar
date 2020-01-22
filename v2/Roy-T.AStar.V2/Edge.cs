namespace Roy_T.AStar.V2
{
    public sealed class Edge : IEdge
    {
        public Edge(INode a, INode b, Velocity traversalVelocity)
        {
            this.A = a;
            this.B = b;
            this.TraversalVelocity = traversalVelocity;
        }

        public Velocity TraversalVelocity { get; set; }
        public INode A { get; }
        public INode B { get; }

        public INode GetOppositeNode(INode oppositeTo) => this.A == oppositeTo ? this.B : this.A;
    }
}

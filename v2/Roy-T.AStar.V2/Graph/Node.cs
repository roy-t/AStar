using System.Collections.Generic;

namespace Roy_T.AStar.V2.Graph
{
    public sealed class Node : INode
    {
        public Node(float x, float y)
        {
            this.X = x;
            this.Y = y;
            this.Incoming = new List<IEdge>(0);
            this.Outgoing = new List<IEdge>(0);
        }

        public IList<IEdge> Incoming { get; }
        public IList<IEdge> Outgoing { get; }

        public float X { get; }
        public float Y { get; }

        public void Connect(INode node, Velocity traversalVelocity)
        {
            var edge = new Edge(this, node, traversalVelocity);
            this.Outgoing.Add(edge);
            node.Incoming.Add(edge);
        }

        public override string ToString() => $"({this.X:F2}, {this.Y:F2})";
    }
}

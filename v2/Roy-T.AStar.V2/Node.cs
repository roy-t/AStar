using System.Collections.Generic;

namespace Roy_T.AStar.V2
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
            var outgoingEdge = new Edge(this, node, traversalVelocity);
            this.Outgoing.Add(outgoingEdge);

            var incomingEdge = new Edge(node, this, traversalVelocity);
            node.Incoming.Add(incomingEdge);
        }

        public IEdge GetEdgeTo(INode node)
        {
            for (var i = 0; i < this.Outgoing.Count; i++)
            {
                var edge = this.Outgoing[i];
                var opposite = edge.GetOppositeNode(this);
                if (opposite == node)
                {
                    return edge;
                }
            }

            return null;
        }

        public IEdge GetEdgeFrom(INode node)
        {
            for (var i = 0; i < this.Incoming.Count; i++)
            {
                var edge = this.Incoming[i];
                var opposite = edge.GetOppositeNode(this);
                if (opposite == node)
                {
                    return edge;
                }
            }

            return null;
        }


        public override string ToString() => $"({this.X:F2}, {this.Y:F2})";
    }
}

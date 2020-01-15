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

        public void Connect(INode node, float cost)
        {
            var edge = new Edge(this, node, cost);
            this.Outgoing.Add(edge);
            node.Incoming.Add(edge);
        }
    }
}

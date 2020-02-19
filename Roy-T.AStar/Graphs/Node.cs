using System.Collections.Generic;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Graphs
{
    public sealed class Node : INode
    {
        public Node(Position position)
        {
            this.Incoming = new List<IEdge>(0);
            this.Outgoing = new List<IEdge>(0);

            this.Position = position;
        }

        public IList<IEdge> Incoming { get; }
        public IList<IEdge> Outgoing { get; }

        public Position Position { get; }

        internal void Connect(INode node, Velocity traversalVelocity)
        {
            var edge = new Edge(this, node, traversalVelocity);
            this.Outgoing.Add(edge);
            node.Incoming.Add(edge);
        }

        internal void Disconnect(INode node)
        {
            for (var i = this.Outgoing.Count - 1; i >= 0; i--)
            {
                var edge = this.Outgoing[i];
                if (edge.End == node)
                {
                    this.Outgoing.Remove(edge);
                    node.Incoming.Remove(edge);
                }
            }
        }

        public override string ToString() => this.Position.ToString();
    }
}

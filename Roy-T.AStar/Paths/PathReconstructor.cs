using System.Collections.Generic;
using Roy_T.AStar.Graphs;

namespace Roy_T.AStar.Paths
{
    internal sealed class PathReconstructor
    {
        private readonly Dictionary<INode, IEdge> CameFrom;

        public PathReconstructor()
        {
            this.CameFrom = new Dictionary<INode, IEdge>();
        }

        public void SetCameFrom(INode node, IEdge via)
            => this.CameFrom[node] = via;

        public Path ConstructPathTo(INode node, INode goal)
        {
            var current = node;
            var edges = new List<IEdge>();

            while (this.CameFrom.TryGetValue(current, out var via))
            {
                edges.Add(via);
                current = via.Start;
            }

            edges.Reverse();

            var type = node == goal ? PathType.Complete : PathType.ClosestApproach;
            return new Path(type, edges);
        }

        public void Clear() => this.CameFrom.Clear();
    }
}

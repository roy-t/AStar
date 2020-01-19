using System;
using System.Collections.Generic;

namespace Roy_T.AStar.V2
{
    public static class PathFinder
    {
        public static Path FindPath(INode start, INode goal, Velocity maximumVelocity)
        {
            if (start == goal)
            {
                return new Path(PathType.Complete, Duration.Zero, new List<IEdge>());
            }

            var open = new MinHeap();
            var nodes = new Dictionary<INode, MinHeapNode>();

            var head = new MinHeapNode(start, null, null, Duration.Zero, ExpectedTime(start, goal, maximumVelocity));
            open.Push(head);
            nodes.Add(head.Node, head);

            var closestApproach = head;

            while (open.HasNext)
            {
                var current = open.Pop();
                if (current.Node == goal)
                {
                    return ReconstructPath(PathType.Complete, current);
                }

                if (current.ExpectedRemainingTime < closestApproach.ExpectedRemainingTime)
                {
                    closestApproach = current;
                }

                foreach (var edge in current.Node.Outgoing)
                {
                    var nextJunction = edge.GetOppositeNode(current.Node);
                    var costSoFar = current.TimeSoFar + ExpectedTime(edge);

                    if (nodes.TryGetValue(nextJunction, out var node))
                    {
                        if (node.TimeSoFar > costSoFar)
                        {
                            node = new MinHeapNode(nextJunction, current, edge, costSoFar, ExpectedTime(nextJunction, goal, maximumVelocity));
                            open.Push(node);
                            nodes[nextJunction] = node;
                        }
                    }
                    else
                    {
                        node = new MinHeapNode(nextJunction, current, edge, costSoFar, ExpectedTime(nextJunction, goal, maximumVelocity));
                        open.Push(node);
                        nodes.Add(node.Node, node);
                    }
                }
            }

            return ReconstructPath(PathType.ClosestApproach, closestApproach);
        }

        private static Path ReconstructPath(PathType type, MinHeapNode node)
        {
            var edges = new List<IEdge>();

            var current = node;
            while (current != null && current.CameVia != null)
            {
                edges.Add(current.CameVia);
                current = current.CameFrom;
            }

            edges.Reverse();

            return new Path(type, node.TimeSoFar, edges);
        }

        private static Duration ExpectedTime(IEdge edge)
            => ExpectedTime(edge.A.X, edge.A.Y, edge.B.X, edge.B.Y, edge.TraversalVelocity);

        private static Duration ExpectedTime(float sX, float sY, float eX, float eY, Velocity velocity)
        {
            var distance = Distance(sX, sY, eX, eY);
            return Duration.FromSeconds(distance / velocity.MetersPerSecond);
        }

        private static float Distance(float sX, float sY, float eX, float eY)
        {
            var d0 = (eX - sX) * (eX - sX);
            var d1 = (eY - sY) * (eY - sY);

            return (float)Math.Sqrt(d0 + d1);
        }

        private static Duration ExpectedTime(INode from, INode to, Velocity maximumVelocity) => ExpectedTime(from.X, from.Y, to.X, to.Y, maximumVelocity);
    }
}

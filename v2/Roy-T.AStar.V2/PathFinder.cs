using System.Collections.Generic;

namespace Roy_T.AStar.V2
{
    public static class PathFinder
    {
        public static Path FindPath(INode start, INode goal, Velocity maximumVelocity)
        {
            if (start == goal)
            {
                return new Path(PathType.Complete, Duration.Zero, 0.0f, new List<IEdge>());
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
                    var oppositeNode = edge.End;
                    var costSoFar = current.TimeSoFar + ExpectedTime(edge);

                    if (nodes.TryGetValue(oppositeNode, out var node))
                    {
                        if (node.TimeSoFar > costSoFar)
                        {
                            node = new MinHeapNode(oppositeNode, current, edge, costSoFar, ExpectedTime(oppositeNode, goal, maximumVelocity));
                            open.Push(node);
                            nodes[oppositeNode] = node;
                        }
                    }
                    else
                    {
                        node = new MinHeapNode(oppositeNode, current, edge, costSoFar, ExpectedTime(oppositeNode, goal, maximumVelocity));
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
            var distance = 0.0f;
            while (current != null && current.CameVia != null)
            {
                var edge = current.CameVia;
                edges.Add(edge);
                distance += MathUtil.Distance(edge.Start.X, edge.Start.Y, edge.End.X, edge.End.Y);

                current = current.CameFrom;
            }

            edges.Reverse();

            return new Path(type, node.TimeSoFar, distance, edges);
        }

        private static Duration ExpectedTime(IEdge edge)
            => MathUtil.ExpectedTime(edge.Start.X, edge.Start.Y, edge.End.X, edge.End.Y, edge.TraversalVelocity);



        private static Duration ExpectedTime(INode from, INode to, Velocity maximumVelocity) => MathUtil.ExpectedTime(from.X, from.Y, to.X, to.Y, maximumVelocity);
    }
}

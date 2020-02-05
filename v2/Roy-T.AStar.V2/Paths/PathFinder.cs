using System.Collections.Generic;
using Roy_T.AStar.V2.Collections;
using Roy_T.AStar.V2.Graphs;
using Roy_T.AStar.V2.Primitives;

namespace Roy_T.AStar.V2.Paths
{
    public class PathFinder
    {
        private readonly MinHeap<MinHeapNode> Interesting;
        private readonly Dictionary<INode, MinHeapNode> Nodes;
        private readonly PathReconstructor PathReconstructor;

        private MinHeapNode NodeClosestToGoal;

        public PathFinder()
        {
            this.Interesting = new MinHeap<MinHeapNode>();
            this.Nodes = new Dictionary<INode, MinHeapNode>();
            this.PathReconstructor = new PathReconstructor();
        }

        public Path FindPath(INode start, INode goal, Velocity maximumVelocity)
        {
            this.ResetState();
            this.AddFirstNode(start, goal, maximumVelocity);

            while (this.Interesting.Count > 0)
            {
                var current = this.Interesting.Extract();
                if (GoalReached(goal, current))
                {
                    return this.PathReconstructor.ConstructPathTo(current.Node, goal);
                }

                this.UpdateNodeClosestToGoal(current);

                foreach (var edge in current.Node.Outgoing)
                {
                    var oppositeNode = edge.End;
                    var costSoFar = current.DurationSoFar + edge.TraversalDuration;

                    if (this.Nodes.TryGetValue(oppositeNode, out var node))
                    {
                        this.UpdateExistingNode(goal, maximumVelocity, current, edge, oppositeNode, costSoFar, node);
                    }
                    else
                    {
                        this.InsertNode(oppositeNode, edge, goal, costSoFar, maximumVelocity);
                    }
                }
            }

            return this.PathReconstructor.ConstructPathTo(this.NodeClosestToGoal.Node, goal);
        }

        private void ResetState()
        {
            this.Interesting.Clear();
            this.Nodes.Clear();
            this.PathReconstructor.Clear();
            this.NodeClosestToGoal = null;
        }

        private void AddFirstNode(INode start, INode goal, Velocity maximumVelocity)
        {
            var head = new MinHeapNode(start, Duration.Zero, ExpectedDuration(start, goal, maximumVelocity));
            this.Interesting.Insert(head);
            this.Nodes.Add(head.Node, head);
            this.NodeClosestToGoal = head;
        }

        private static bool GoalReached(INode goal, MinHeapNode current) => current.Node == goal;

        private void UpdateNodeClosestToGoal(MinHeapNode current)
        {
            if (current.ExpectedRemainingTime < this.NodeClosestToGoal.ExpectedRemainingTime)
            {
                this.NodeClosestToGoal = current;
            }
        }

        private void UpdateExistingNode(INode goal, Velocity maximumVelocity, MinHeapNode current, IEdge edge, INode oppositeNode, Duration costSoFar, MinHeapNode node)
        {
            if (node.DurationSoFar > costSoFar)
            {
                this.Interesting.Remove(node);
                this.InsertNode(oppositeNode, edge, goal, costSoFar, maximumVelocity);
            }
        }

        private void InsertNode(INode current, IEdge via, INode goal, Duration costSoFar, Velocity maximumVelocity)
        {
            this.PathReconstructor.SetCameFrom(current, via);

            var node = new MinHeapNode(current, costSoFar, ExpectedDuration(current, goal, maximumVelocity));
            this.Interesting.Insert(node);
            this.Nodes[current] = node;
        }

        public static Duration ExpectedDuration(INode a, INode b, Velocity maximumVelocity)
            => Distance.BeweenPositions(a.Position, b.Position) / maximumVelocity;
    }
}

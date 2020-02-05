using System;
using Roy_T.AStar.V2.Graphs;
using Roy_T.AStar.V2.Primitives;

namespace Roy_T.AStar.V2.Paths
{
    public sealed class MinHeapNode : IComparable<MinHeapNode>
    {
        public MinHeapNode(INode node, Duration durationSoFar, Duration expectedRemainingTime)
        {
            this.Node = node;
            this.DurationSoFar = durationSoFar;
            this.ExpectedRemainingTime = expectedRemainingTime;
            this.ExpectedTotalTime = this.DurationSoFar + this.ExpectedRemainingTime;
        }

        public INode Node { get; }
        public Duration DurationSoFar { get; }
        public Duration ExpectedRemainingTime { get; }
        public Duration ExpectedTotalTime { get; }

        public int CompareTo(MinHeapNode other) => this.ExpectedTotalTime.CompareTo(other.ExpectedTotalTime);
        public override string ToString() => $"📍{{{this.Node.Position.X}, {this.Node.Position.Y}}}, ⏱~{this.ExpectedTotalTime}";
    }
}

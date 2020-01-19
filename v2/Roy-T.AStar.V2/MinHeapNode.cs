namespace Roy_T.AStar.V2
{
    internal sealed class MinHeapNode
    {
        public MinHeapNode(INode node, MinHeapNode cameFrom, IEdge cameVia, Duration timeSoFar, Duration expectedRemainingTime)
        {
            this.Node = node;
            this.CameFrom = cameFrom;
            this.CameVia = cameVia;
            this.TimeSoFar = timeSoFar;
            this.ExpectedRemainingTime = expectedRemainingTime;
        }

        public INode Node { get; }
        public MinHeapNode CameFrom { get; }
        public IEdge CameVia { get; }
        public Duration TimeSoFar { get; }
        public Duration ExpectedRemainingTime { get; }

        public Duration ExpectedTotalTime => this.TimeSoFar + ExpectedRemainingTime;
        public MinHeapNode Next { get; set; }

        public override string ToString() => $"📍{{{this.Node.X}, {this.Node.Y}}}, ⏱~{this.ExpectedTotalTime}";
    }
}

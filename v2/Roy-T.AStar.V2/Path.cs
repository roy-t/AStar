using System.Collections.Generic;

namespace Roy_T.AStar.V2
{
    public class Path
    {
        public Path(PathType type, Duration expectedDuration, IReadOnlyList<IEdge> edges)
        {
            this.Type = type;
            this.ExpectedDuration = expectedDuration;
            this.Edges = edges;
        }

        public PathType Type { get; }

        public Duration ExpectedDuration { get; }

        public IReadOnlyList<IEdge> Edges { get; }
    }
}

using System.Collections.Generic;

namespace Roy_T.AStar.V2.Graph
{
    public interface INode
    {
        float X { get; }
        float Y { get; }

        IList<IEdge> Incoming { get; }
        IList<IEdge> Outgoing { get; }
    }
}

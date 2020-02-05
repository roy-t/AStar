using System.Collections.Generic;
using Roy_T.AStar.V2.Primitives;

namespace Roy_T.AStar.V2.Graphs
{
    public interface INode
    {
        Position Position { get; }
        IList<IEdge> Incoming { get; }
        IList<IEdge> Outgoing { get; }
    }
}

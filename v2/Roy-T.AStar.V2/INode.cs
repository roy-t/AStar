using System.Collections.Generic;

namespace Roy_T.AStar.V2
{
    public interface INode
    {
        float X { get; }
        float Y { get; }

        IList<IEdge> Incoming { get; }
        IList<IEdge> Outgoing { get; }

        void Connect(INode node, float cost);
    }
}

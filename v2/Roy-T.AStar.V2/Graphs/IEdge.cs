using Roy_T.AStar.V2.Primitives;

namespace Roy_T.AStar.V2.Graphs
{
    public interface IEdge
    {
        Velocity TraversalVelocity { get; set; }
        Duration TraversalDuration { get; }
        Distance Distance { get; }
        INode Start { get; }
        INode End { get; }
    }
}
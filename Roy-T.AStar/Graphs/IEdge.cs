using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Graphs
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
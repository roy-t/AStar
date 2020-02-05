namespace Roy_T.AStar.V2.Graph
{
    public interface IEdge
    {
        Velocity TraversalVelocity { get; set; }
        Duration TraversalDuration { get; }

        INode Start { get; }
        INode End { get; }
    }
}
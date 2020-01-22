namespace Roy_T.AStar.V2
{
    public interface IEdge
    {
        Velocity TraversalVelocity { get; set; }

        INode Start { get; }
        INode End { get; }
    }
}
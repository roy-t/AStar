namespace Roy_T.AStar.V2
{
    public interface IEdge
    {
        float TraversalCost { get; }

        INode Start { get; }
        INode End { get; }
    }
}
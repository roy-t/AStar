namespace Roy_T.AStar.V2
{
    public interface IEdge
    {
        Velocity TraversalVelocity { get; }

        INode A { get; }
        INode B { get; }

        INode GetOppositeNode(INode node);
    }
}
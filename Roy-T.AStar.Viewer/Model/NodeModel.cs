using ReactiveUI;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Primitives;

namespace Roy_T.AStar.Viewer.Model
{
    internal sealed class NodeModel : ReactiveObject
    {
        private NodeState nodeState;

        public NodeModel(INode node, GridPosition gridPosition)
        {
            this.Node = node;
            this.GridPosition = gridPosition;
            this.nodeState = NodeState.None;
        }

        public INode Node { get; }
        public GridPosition GridPosition { get; }

        public float X => this.Node.Position.X;
        public float Y => this.Node.Position.Y;

        public NodeState NodeState
        {
            get => this.nodeState;
            set => this.RaiseAndSetIfChanged(ref this.nodeState, value);
        }

        public float Z => 2;

        public IReactiveCommand LeftClickCommand { get; set; }
        public IReactiveCommand RightClickCommand { get; set; }
    }
}

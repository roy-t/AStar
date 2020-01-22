using ReactiveUI;
using Roy_T.AStar.V2;

namespace Roy_T.AStar.Viewer.Model
{
    internal sealed class NodeModel : ReactiveObject
    {
        private NodeState nodeState;

        public NodeModel(INode node)
        {
            this.Node = node;
            this.nodeState = NodeState.None;
        }

        public INode Node { get; }

        public float X => this.Node.X;
        public float Y => this.Node.Y;

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

using ReactiveUI;
using Roy_T.AStar.V2;

namespace Roy_T.AStar.Viewer.Model
{
    internal sealed class NodeModel : ReactiveObject
    {
        public NodeModel(INode node)
        {
            this.Node = node;
        }

        public INode Node { get; }

        public float X => this.Node.X;
        public float Y => this.Node.Y;

        public float Z => 1;

        public IReactiveCommand ClickCommand { get; set; }
    }
}

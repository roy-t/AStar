using ReactiveUI;
using Roy_T.AStar.V2;

namespace Roy_T.AStar.Viewer.Model
{
    internal abstract class AEdgeModel : ReactiveObject
    {
        private readonly IEdge Edge;

        public AEdgeModel(IEdge edge)
        {
            this.Edge = edge;
        }

        public Velocity Velocity
        {
            get => this.Edge.TraversalVelocity;
            set
            {
                this.Edge.TraversalVelocity = value;
                this.RaisePropertyChanged(nameof(this.Velocity));
            }
        }

        public float X1 => this.Edge.A.X;
        public float Y1 => this.Edge.A.Y;
        public float X2 => this.Edge.B.X;
        public float Y2 => this.Edge.B.Y;

        public float Z => 0;
    }
}

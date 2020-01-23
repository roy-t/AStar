using ReactiveUI;
using Roy_T.AStar.V2;

namespace Roy_T.AStar.Viewer.Model
{
    internal class EdgeModel : ReactiveObject
    {
        public EdgeModel(IEdge edge)
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

        public IEdge Edge { get; }

        public float X1 => this.Edge.Start.X;
        public float Y1 => this.Edge.Start.Y;
        public float X2 => this.Edge.End.X;
        public float Y2 => this.Edge.End.Y;

        public float Z => 1;

        // To prevent binding errors (or complicated content presenter logic) we also define an X and Y component on edges
        public float X => 0;
        public float Y => 0;
    }
}

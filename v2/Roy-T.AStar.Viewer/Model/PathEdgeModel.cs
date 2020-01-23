using ReactiveUI;

namespace Roy_T.AStar.Viewer.Model
{
    internal sealed class PathEdgeModel : ReactiveObject
    {
        public PathEdgeModel(float x1, float y1, float x2, float y2)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
        }

        public float X1 { get; }
        public float Y1 { get; }
        public float X2 { get; }
        public float Y2 { get; }

        public float Z => 0;

        // To prevent binding errors (or complicated content presenter logic) we also define an X and Y component on edges
        public float X => 0;
        public float Y => 0;
    }
}

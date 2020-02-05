namespace Roy_T.AStar.V2.Primitives
{
    public struct Position
    {
        public static Position Zero => new Position(0, 0);

        public Position(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float X { get; }
        public float Y { get; }

        public override string ToString() => $"({this.X:F2}, {this.Y:F2})";
    }
}
